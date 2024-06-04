using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 5f;
    public float minimumDistance = 3f; // Minimum distance to maintain from the player
    public float fireRate = 1f;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileSpeed = 20f; // Increase the speed of the projectile

    public float maxHealth = 100f;
    private float currentHealth;

    private NavMeshAgent agent;
    private float nextFireTime;
    private State currentState;

    private enum State { Idle, Chase, Attack, Flee, TakeCover }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
        currentState = State.Idle;
    }

    void FixedUpdate()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                Idle(distanceToPlayer);
                break;
            case State.Chase:
                Chase(distanceToPlayer);
                break;
            case State.Attack:
                Attack(distanceToPlayer);
                break;
            case State.Flee:
                Flee();
                break;
            case State.TakeCover:
                TakeCover();
                break;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Idle(float distanceToPlayer)
    {
        if (distanceToPlayer <= detectionRange)
        {
            currentState = State.Chase;
        }
    }

    void Chase(float distanceToPlayer)
    {
        if (distanceToPlayer > minimumDistance)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.SetDestination(transform.position); // Stop moving if too close
        }
        RotateTowards(player.position); // Rotate to face the player

        if (distanceToPlayer <= attackRange)
        {
            currentState = State.Attack;
        }
        else if (currentHealth <= maxHealth * 0.3f)
        {
            currentState = State.Flee;
        }
    }

    void Attack(float distanceToPlayer)
    {
        agent.SetDestination(transform.position); // Stop moving to attack
        RotateTowards(player.position); // Rotate to face the player

        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }

        if (distanceToPlayer > attackRange)
        {
            currentState = State.Chase;
        }
        else if (currentHealth <= maxHealth * 0.3f)
        {
            currentState = State.Flee;
        }
    }

    void RotateTowards(Vector3 target)
    {
        // Determine which direction to rotate towards
        Vector3 direction = (target - transform.position).normalized;

        // Create the rotation we need to be in to look at the target
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Rotate the enemy to face the player
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void Shoot()
    {
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            // Predict the player's future position
            Vector3 predictedPosition = PredictPlayerPosition();

            // Calculate the direction to the predicted position
            Vector3 directionToPlayer = (predictedPosition - projectileSpawnPoint.position).normalized;

            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.LookRotation(directionToPlayer));

            // Add logic to move the projectile forward
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = directionToPlayer * projectileSpeed;
            }
        }
    }

    Vector3 PredictPlayerPosition()
    {
        // Improved prediction logic based on player's current velocity
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            float distanceToPlayer = Vector3.Distance(projectileSpawnPoint.position, player.position);
            float timeToTarget = distanceToPlayer / projectileSpeed;
            return player.position + playerRb.velocity * timeToTarget;
        }
        return player.position;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        // Add logic to react to taking damage (e.g., evade or flee)
        if (currentHealth <= maxHealth * 0.3f)
        {
            // Example: If health drops below 30%, try to flee
            currentState = State.TakeCover;
        }
    }

    void Die()
    {
        // Add death behavior here (e.g., play death animation, drop loot, etc.)
        Destroy(gameObject);
    }

    void Flee()
    {
        Vector3 fleeDirection = transform.position - player.position;
        fleeDirection.Normalize();
        Vector3 fleePosition = transform.position + fleeDirection * 10f; // Flee distance
        agent.SetDestination(fleePosition);

        if (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            currentState = State.Idle;
        }
    }

    void TakeCover()
    {
        Vector3 coverPosition = FindCoverPosition();
        agent.SetDestination(coverPosition);

        if (Vector3.Distance(transform.position, coverPosition) < 1f)
        {
            currentState = State.Idle;
        }
    }

    Vector3 FindCoverPosition()
    {
        // Implement logic to find a suitable cover position
        // For simplicity, let's just move the enemy away from the player
        Vector3 coverDirection = transform.position - player.position;
        coverDirection.Normalize();
        return transform.position + coverDirection * 5f;
    }
}

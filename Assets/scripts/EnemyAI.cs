using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 5f;
    public float fireRate = 1f;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    public float maxHealth = 100f;
    private float currentHealth;

    private NavMeshAgent agent;
    private float nextFireTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            agent.SetDestination(player.position);

            if (distanceToPlayer <= attackRange)
            {
                if (Time.time >= nextFireTime)
                {
                    Shoot();
                    nextFireTime = Time.time + 1f / fireRate;
                }
            }
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Shoot()
    {
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            // Add logic to aim the projectile at the player
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        // Add logic to react to taking damage (e.g., evade or flee)
        if (currentHealth <= maxHealth * 0.3f)
        {
            // Example: If health drops below 30%, try to flee
            Flee();
        }
    }

    void Die()
    {
        // Add death behavior here (e.g., play death animation, drop loot, etc.)
        Destroy(gameObject);
    }

    void Flee()
    {
        // Add fleeing behavior here (e.g., move away from player)
        Vector3 fleeDirection = transform.position - player.position;
        fleeDirection.Normalize();
        Vector3 fleePosition = transform.position + fleeDirection * 10f; // Flee distance
        agent.SetDestination(fleePosition);
    }
}

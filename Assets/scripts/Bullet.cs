using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 10f;
    float destroyDelay = 2f; // Adjust this value to set the delay before the bullet is destroyed
    public bool IsEnemyBullet = false;
    public Gun firedFrom = null;
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Start()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(IsEnemyBullet)
        {
            var playerComp = collision.gameObject.GetComponent<Playercontroller>();
            if (playerComp != null)
            {
                playerComp.TakeDamage(firedFrom.bulletDamage);
            }
        }
        else
        {
            var enemyComp = collision.gameObject.GetComponent<EnemyAI>();
            if (enemyComp != null)
            {
                enemyComp.TakeDamage(firedFrom.bulletDamage);
            }
        }
       
    }
}

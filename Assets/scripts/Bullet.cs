using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 10f;
    float destroyDelay = 2f; // Adjust this value to set the delay before the bullet is destroyed

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
}

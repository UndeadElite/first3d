using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform muzzle;
    public Bullet bulletPrefab;
    public float bulletSpeed = 20f;
    public float fireRate = 1f;

    private float lastShotTime;

    public void Shoot()
    {
        if (Time.time > lastShotTime + 1 / fireRate)
        {
            lastShotTime = Time.time;
            Bullet newBullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
            newBullet.SetSpeed(bulletSpeed);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform muzzle;
    public Bullet bulletPrefab;
    public float bulletSpeed = 20f;
    public float fireRate = 1f;
    public float bulletDamage = 1f;
    private float lastShotTime;
    public bool IsEnemyGun = false; 

    public void Shoot()
    {
        if (Time.time > lastShotTime + 1 / fireRate)
        {
            lastShotTime = Time.time;
            Bullet newBullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
            newBullet.GetComponent<Bullet>().firedFrom = this;
            newBullet.GetComponent<Bullet>().IsEnemyBullet = IsEnemyGun;

            newBullet.SetSpeed(bulletSpeed);
        }
    }
}

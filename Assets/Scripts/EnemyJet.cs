using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJet : BasicEnemy
{
    [SerializeField] float volleySize;
    [SerializeField] float volleyTime;
    [SerializeField] float bulletTime;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletDamage;
    [SerializeField] float bulletSpread;
    private EnemyBullet bulletToSpawn;
    [SerializeField] ParticleSystem particleSystemFire1;
    [SerializeField] ParticleSystem particleSystemFire2;

    public override void OnStart()
    {
        bulletToSpawn = gameController.enemyBullet;
        StartCoroutine(VolleySpawning());
        particleSystemFire1.Play();
        particleSystemFire2.Play();
    }

    public override void Frame()
    {
        transform.Translate(new Vector3(0, -1, 0) * speed * Time.deltaTime *data.scaling);
    }
    public override float CalculateHealth(int wave)
    {
        return 8;
    }
    public override float CalculateSpeed(int wave)
    {
        return wave * 0 + 100;
    }
    public override float CalculateDamage(int wave)
    {
        return 3;
    }
    public override int CalculateEnemiesToSpawn(int wave)
    {
        return 3 + (int)System.Math.Floor(0.5 * wave);
    }
    public override float CalculateSpawnTime(int wave)
    {
        return 6f / CalculateEnemiesToSpawn(wave);
    }

    IEnumerator VolleySpawning()
    {
        yield return new WaitForSeconds(volleyTime * Random.Range(0f, volleyTime));
        for (; ; )
        {
            StartCoroutine(BulletSpawning());
            yield return new WaitForSeconds(volleyTime);
        }
    }

    IEnumerator BulletSpawning()
    {
        for (int i = 0; i<volleySize ; i++ )
        {
            if (!data.isPaused)
            {
                EnemyBullet spawnedBullet = Instantiate(bulletToSpawn, transform.position, Quaternion.Euler(0, 0, 0), gameController.enemyBulletParent);
                spawnedBullet.speed = bulletSpeed;
                spawnedBullet.damage = bulletDamage;
                spawnedBullet.spread = bulletSpread;
                spawnedBullet.gameController = gameController;
                spawnedBullet.data = data;
                yield return new WaitForSeconds(bulletTime);
            }
        }
    }
}

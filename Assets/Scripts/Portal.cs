using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : BasicEnemy
{
    [SerializeField] GameObject outer;
    [SerializeField] GameObject inner;
    [SerializeField] GameObject mist;
    [SerializeField] float outerRotation;
    [SerializeField] float innerRotation;
    [SerializeField] float mistRotation;
    [SerializeField] float spawnTime;

    private void Start()
    {
        StartCoroutine(MeteoriteSpawning());
        mist.transform.Rotate(0, 0, Random.Range(0, 360));
    }

    public override void Frame()
    {
        transform.Translate(new Vector3(0,-1,0) * speed * Time.deltaTime);
        outer.transform.Rotate(0,0,outerRotation * Time.deltaTime);
        inner.transform.Rotate(0,0,innerRotation * Time.deltaTime);
        mist.transform.Rotate(0,0,mistRotation * Time.deltaTime);
    }
    public override float CalculateHealth(int wave)
    {
        return 5;
    }
    public override float CalculateSpeed(int wave)
    {
        return wave * 10 + 350;
    }
    public override float CalculateDamage(int wave)
    {
        return 2;
    }
    public override int CalculateEnemiesToSpawn(int wave)
    {
        return 8 + 2 * wave;
    }
    public override float CalculateSpawnTime(int wave)
    {
        return 5f / CalculateEnemiesToSpawn(wave);
    }

    IEnumerator MeteoriteSpawning()
    {
        yield return new WaitForSeconds(spawnTime*Random.Range(0f, spawnTime));
        for (; ; )
        {
            if (!data.isPaused)
            {
                BasicEnemy enemyToSpawn = gameController.meteorite;
                BasicEnemy spawnedEnemy = Instantiate(enemyToSpawn, transform.position, Quaternion.Euler(0, 0, 0), gameController.enemiesParent);
                spawnedEnemy.health = spawnedEnemy.CalculateHealth(gameController.wave);
                spawnedEnemy.speed = spawnedEnemy.CalculateSpeed(gameController.wave) / 3;
                spawnedEnemy.damage = spawnedEnemy.CalculateDamage(gameController.wave);
                spawnedEnemy.gameController = gameController;
                spawnedEnemy.data = data;
                yield return new WaitForSeconds(spawnTime);
            }
        }
    }
}

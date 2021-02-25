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
    }

    public override void Frame()
    {
        transform.Translate(new Vector3(0,-1,0) * speed * Time.deltaTime);
        outer.transform.Rotate(0,0,outerRotation * Time.deltaTime);
        inner.transform.Rotate(0,0,innerRotation * Time.deltaTime);
        inner.transform.Rotate(0,0,innerRotation * Time.deltaTime);
    }
    public override float CalculateHealth(int wave)
    {
        return 5;
    }
    public override float CalculateSpeed(int wave)
    {
        return wave * 20 + 400;
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
            BasicEnemy enemyToSpawn = gameController.meteorite;
            int enemiesToSpawn = enemyToSpawn.CalculateEnemiesToSpawn(gameController.wave);
            BasicEnemy spawnedEnemy = Instantiate(enemyToSpawn, transform.position, Quaternion.Euler(0, 0, 0), gameController.enemiesParent);
            spawnedEnemy.health = spawnedEnemy.CalculateHealth(gameController.wave); // 1;
            spawnedEnemy.speed = spawnedEnemy.CalculateSpeed(gameController.wave)/3; // wave*100+1500;
            spawnedEnemy.damage = spawnedEnemy.CalculateDamage(gameController.wave); // 1;
            spawnedEnemy.gameController = gameController;
            spawnedEnemy.data = data;
            yield return new WaitForSeconds(spawnTime);
        }
    }
}

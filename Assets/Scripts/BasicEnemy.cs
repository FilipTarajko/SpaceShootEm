using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicEnemy : MonoBehaviour
{
    public GameController gameController;
    public Data data;
    public float health;
    public float speed;
    public float damage;
    public float powerUpPercentChance;
    public Vector3 movementVector = Vector3.down;
    public Color ColorForParticles;

    private void Start()
    {
        OnStart();
        transform.localScale *= data.scaling;
    }
    public abstract void OnStart();
    void Update()
    {
        if ((!data.CheckIfOut(transform)) && health > 0)
        {
            if (!data.isPaused)
            {
                Frame();
            }
        }
        else if(health > 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Death();
        }
    }
    public abstract void Frame();
    public void Death()
    {
        if (powerUpPercentChance > Random.Range(0f, 100f))
        {
            PowerUp toSpawn = gameController.powerUp;
            PowerUp spawnedEnemy = Instantiate(toSpawn, transform.position, Quaternion.Euler(0, 0, 0), gameController.powerUpsParent);
            spawnedEnemy.speed = data.powerUpSpeed;
            spawnedEnemy.gameController = gameController;
            spawnedEnemy.data = data;
        }
        //BasicEnemy spawnedEnemy = Instantiate(enemyToSpawn, new Vector3(spawnX,spawnY,0), Quaternion.Euler(0,0,0), enemiesParent);
        EnemyDestroyedParticleParent enemyDestroyedParticleParent = Instantiate(gameController.enemyDestroyedParticleParent, transform.position, Quaternion.Euler(0, 0, 0), gameController.enemyParticlesParent);
        enemyDestroyedParticleParent.ColorForParticles = ColorForParticles;
        enemyDestroyedParticleParent.data = data;
        if (movementVector != null)
        {
            enemyDestroyedParticleParent.movementVector = movementVector;
        }
        enemyDestroyedParticleParent.speed = speed;
        Destroy(gameObject);
    }

    public void DealDamage(float dealtDamage)
    {
        health -= dealtDamage;
    }


    public abstract float CalculateHealth(int wave);
    public abstract float CalculateSpeed(int wave);
    public abstract float CalculateDamage(int wave);
    public abstract int CalculateEnemiesToSpawn(int wave);
    public abstract float CalculateSpawnTime(int wave);
}

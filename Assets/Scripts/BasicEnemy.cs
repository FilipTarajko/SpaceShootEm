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
        else if(health > 0) //separate death from going off-screen and being removed
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

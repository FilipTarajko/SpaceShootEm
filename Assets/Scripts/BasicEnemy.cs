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

    void Update()
    {
        if (transform.position.y > -(Screen.height*(0.5+data.entityBorder)) && health > 0)
        {
            Frame();
        }
        else
        {
            Death();
        }
    }
    public abstract void Frame();
    public void Death()
    {
        Destroy(this.gameObject);
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

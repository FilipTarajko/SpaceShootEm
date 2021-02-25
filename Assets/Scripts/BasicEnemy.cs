using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicEnemy : MonoBehaviour
{
    public GameController gameController;
    public Data data;
    public double health;
    public float speed;
    public float damage;

    void Update()
    {
        if (transform.position.y > -(Screen.height*(0.5+data.entityBorder)) && health > 0)
        {
            Movement();
        }
        else
        {
            Death();
        }
    }

    public abstract void Movement();
    public void Death()
    {
        Destroy(this.gameObject);
    }

    public void DealDamage(double dealtDamage)
    {
        health -= dealtDamage;
    }
}

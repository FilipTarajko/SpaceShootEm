using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public GameController gameController;
    public Data data;
    public double health;
    public float speed;
    public double damage;

    void Update()
    {
        if (transform.position.y > -(Screen.height*(0.5+data.entityBorder)) && health > 0)
        {
            transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void DealDamage(double dealtDamage)
    {
        health -= dealtDamage;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public GameController gameController;
    public Data data;
    public double health;
    public float speed;
    public float damage;
    public float angle;
    public Vector3 movementVector;

    private void Start()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(-angle, +angle));
        Vector3 downVector = new Vector3(0, -1, 0);
        movementVector = rotation * downVector;
    }

    void Update()
    {
        if (transform.position.y > -(Screen.height*(0.5+data.entityBorder)) && health > 0)
        {
            transform.Translate(movementVector * speed * Time.deltaTime);
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

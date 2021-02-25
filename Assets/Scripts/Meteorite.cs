using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : BasicEnemy
{
    public float angle;
    public Vector3 movementVector;

    private void Start()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(-angle, +angle));
        Vector3 downVector = new Vector3(0, -1, 0);
        movementVector = rotation * downVector;
    }

    public override void Movement()
    {
        transform.Translate(movementVector * speed * Time.deltaTime);
    }
}

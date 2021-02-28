using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : BasicEnemy
{
    public float angle;
    public Vector3 movementVector;
    public float minAsteroidScale;
    public float maxAsteroidScale;

    public override void OnStart()
    {
        float scale = Random.Range(minAsteroidScale, maxAsteroidScale);
        transform.localScale = new Vector3(scale, scale, 1f);
        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(-angle, +angle));
        Vector3 downVector = new Vector3(0, -1, 0);
        movementVector = rotation * downVector;
    }

    public override void Frame()
    {
        transform.Translate(movementVector * speed * Time.deltaTime *data.scaling);
    }

    public override float CalculateHealth(int wave)
    {
        return 1;
    }
    public override float CalculateSpeed(int wave)
    {
        return wave * 50 + 1200;
    }
    public override float CalculateDamage(int wave)
    {
        return 1;
    }
    public override int CalculateEnemiesToSpawn(int wave)
    {
        return 50 + 8 * wave;
    }
    public override float CalculateSpawnTime(int wave)
    {
        return 2f / CalculateEnemiesToSpawn(wave);
    }
}

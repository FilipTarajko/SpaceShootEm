using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyedParticleParent : MonoBehaviour
{
    public float speed;
    public Vector3 movementVector;
    public Data data;
    public Color ColorForParticles;
    public ParticleSystem particleSystemDestroyed;

    private void Start()
    {
        var main = particleSystemDestroyed.main;
        main.startColor = ColorForParticles;
    }

    void Update()
    {
        if (!data.isPaused)
        {
            transform.Translate(movementVector * speed * Time.deltaTime * data.scaling);
        }
    }
}

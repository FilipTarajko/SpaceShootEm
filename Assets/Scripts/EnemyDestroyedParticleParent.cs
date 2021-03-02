using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyedParticleParent : MonoBehaviour
{
    public float speed;
    public Vector3 movementVector;
    public Data data;
    public Color ColorForParticles1;
    public Color ColorForParticles2;
    public ParticleSystem particleSystemDestroyed;

    private void Start()
    {
        ParticleSystem.MinMaxGradient gradient = new ParticleSystem.MinMaxGradient(ColorForParticles1, ColorForParticles2);
        //gradient.mode = ParticleSystemGradientMode.RandomColor;
        var main = particleSystemDestroyed.main;
        main.startColor = gradient;
    }

    void Update()
    {
        if (!data.isPaused)
        {
            transform.Translate(movementVector * speed * Time.deltaTime * data.scaling);
        }
    }
}

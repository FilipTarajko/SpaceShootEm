using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public BasicEnemy basicEnemy;
    public GameObject dynamic;
    public int wave;

    void Start()
    {
        StartCoroutine(WaveStarter());
    }

    IEnumerator WaveStarter()
    {
        for (;;)
        {
            wave += 1;
            SpawnWave(wave);
            yield return new WaitForSeconds(5);
        }
    }

    void SpawnWave(int wave)
    {
        for (int i = 0; i < wave; i++)
        {
            float horizontalMargin = 50;
            float spawnX = Random.Range(-Screen.width/2+horizontalMargin, Screen.width/2-horizontalMargin);
            float spawnY = Screen.height;
            BasicEnemy spawnedEnemy = Instantiate(basicEnemy, new Vector3(spawnX,spawnY,0), Quaternion.Euler(0,0,0), dynamic.transform);
            spawnedEnemy.health = wave;
            spawnedEnemy.speed = wave*100+1000;
            spawnedEnemy.damage = 1;
        }
    }
}

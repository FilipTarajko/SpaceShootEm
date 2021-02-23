using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public Data data;
    public Player player;
    public BasicEnemy basicEnemy;
    public GameObject dynamic;
    public int wave;
    public TMP_Text waveDisplay;

    void Start()
    {
        StartCoroutine(WaveStarter());
    }

    IEnumerator WaveStarter()
    {
        for (;;)
        {
            wave += 1;
            UpdateWaveText();
            SpawnWave(wave);
            yield return new WaitForSeconds(3);
        }
    }

    void UpdateWaveText()
    {
        waveDisplay.text = $"Wave: {wave}";
    }

    void SpawnWave(int wave)
    {
        player.GetHealing(0.1f);
        for (int i = 0; i < 15 + 3*wave; i++)
        {
            float horizontalMargin = 50;
            float spawnX = Random.Range(-Screen.width/2+horizontalMargin, Screen.width/2-horizontalMargin);
            float spawnY = Screen.height*(0.5f+data.entityBorder);
            BasicEnemy spawnedEnemy = Instantiate(basicEnemy, new Vector3(spawnX,spawnY,0), Quaternion.Euler(0,0,0), dynamic.transform);
            spawnedEnemy.health = 1;
            spawnedEnemy.speed = wave*100+1500;
            spawnedEnemy.damage = 1;
            spawnedEnemy.gameController = this;
            spawnedEnemy.data = data;
        }
    }
}

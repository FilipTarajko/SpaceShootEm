using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Data data;
    public Player player;
    public BasicEnemy basicEnemy;
    public GameObject dynamic;
    public Transform enemiesParent;
    public int wave;
    public TMP_Text waveDisplay;
    public RedFlash redFlash;

    void Start()
    {
        waveDisplay.color = new Color(1, 1, 1, 0);
        StartCoroutine(WaveStarter());
    }
    public IEnumerator Death()
    {
        waveDisplay.text = "You died!";
        waveDisplay.color = new Color(0.4f, 0, 0, 1);
        StartCoroutine(WaveDisplaySize());
        StartCoroutine(WaveDisplayColor());
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(0);
    }

    IEnumerator WaveStarter()
    {
        yield return new WaitForSeconds(1.5f);
        for (;;)
        {
            if(enemiesParent.childCount == 0 && data.isAlive)
            {
                wave += 1;
                UpdateWaveText();
                StartCoroutine(WaveDisplaySize());
                StartCoroutine(WaveDisplayColor());
                yield return new WaitForSeconds(2.5f);
                StartCoroutine(SpawnWave(wave));
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator WaveDisplayColor()
    {
        Color waveDisplayColor = waveDisplay.color;
        float alpha = 0;
        for (int i = 0; i < 100; i++)
        {
            alpha += 0.01f;
            waveDisplay.color = ChangeAlpha(waveDisplayColor, alpha);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 100; i++)
        {
            alpha -= 0.01f;
            waveDisplay.color = ChangeAlpha(waveDisplayColor, alpha);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private Color ChangeAlpha(Color color, float value)
    {
        return new Color(color.r, color.g, color.b, value);
    }

    public IEnumerator WaveDisplaySize()
    {
        waveDisplay.fontSize = 200f;
        for (int i = 0; i < 250; i++)
        {
            waveDisplay.fontSize += 0.5f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    void UpdateWaveText()
    {
        waveDisplay.text = $"Wave: {wave}";
    }

    IEnumerator SpawnWave(int wave)
    {
        player.GetHealing(0.1f);
        int enemiesToSpawn = data.CalcEnemiesToSpawn(wave);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            float horizontalMargin = 50;
            float spawnX = Random.Range(-Screen.width/2+horizontalMargin, Screen.width/2-horizontalMargin);
            float spawnY = Screen.height*(0.5f+data.entityBorder);
            BasicEnemy spawnedEnemy = Instantiate(basicEnemy, new Vector3(spawnX,spawnY,0), Quaternion.Euler(0,0,0), enemiesParent);
            spawnedEnemy.health = 1;
            spawnedEnemy.speed = wave*100+1500;
            spawnedEnemy.damage = 1;
            spawnedEnemy.gameController = this;
            spawnedEnemy.data = data;
            float scale = Random.Range(data.minAsteroidScale, data.maxAsteroidScale);
            spawnedEnemy.transform.localScale = new Vector3(scale, scale, 1f);
            yield return new WaitForSeconds(2f/(float)enemiesToSpawn);
        }
    }
}

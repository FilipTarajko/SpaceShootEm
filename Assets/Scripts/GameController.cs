using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Data data;
    public Player player;
    public BasicEnemy meteorite;
    public BasicEnemy shootingEnemy;
    public BasicEnemy enemyJet;
    public GameObject dynamic;
    public Transform enemiesParent;
    public Transform powerUpsParent;
    public int wave;
    public TMP_Text waveDisplay;
    public RedFlash redFlash;
    public PowerUp powerUp;
    public Button pauseButton;
    private BasicEnemy enemyToSpawn;

    List<BasicEnemy> enemiesList = new List<BasicEnemy>();

    public void PauseUnpase()
    {
        data.isPaused = !data.isPaused;
        if (data.isPaused)
        {
            Time.timeScale = 0;
            pauseButton.image.color = data.pauseButtonPausedColor;
            pauseButton.transform.localScale *= 2;
            pauseButton.transform.position -= new Vector3(data.pauseButtonWidth/2, data.pauseButtonHeight/2, 0);
        }
        else
        {
            Time.timeScale = 1;
            pauseButton.image.color = data.pauseButtonDefaultColor;
            pauseButton.transform.localScale /= 2;
            pauseButton.transform.position += new Vector3(data.pauseButtonWidth/2, data.pauseButtonHeight/2, 0);
        }
    }

    private IEnumerator PausedButtonAlpha()
    {
        data.pauseButtonTargetAlpha = pauseButton.image.color.a;
        for (;;)
        {
            if (data.pauseButtonTargetAlpha >= 1)
            {
                data.pauseButtonPulseDirection = -1;
                data.pauseButtonTargetAlpha = 1;
            }
            else if (data.pauseButtonTargetAlpha <= data.pauseButtonDefaultColor.a)
            {
                data.pauseButtonPulseDirection = 1;
                data.pauseButtonTargetAlpha = data.pauseButtonDefaultColor.a;
            }
            if (data.isPaused)
            {
                data.pauseButtonTargetAlpha += data.pauseButtonPulseDirection * (1 - data.pauseButtonDefaultColor.a) / data.pauseButtonPulseTime / data.targetFPS;
                pauseButton.image.color = ChangeAlpha(pauseButton.image.color, data.pauseButtonTargetAlpha);
            }
            yield return new WaitForSecondsRealtime(1f / data.targetFPS);
        }
    }

    void Start()
    {
        BasicEnemy[] enemies = { meteorite, shootingEnemy, enemyJet };
        for (int i = 0; i < enemies.Length; i++)
        {
            enemiesList.Add(enemies[i]);
        }
        waveDisplay.color = new Color(1, 1, 1, 0);
        StartCoroutine(WaveStarter());
        pauseButton.image.color = data.pauseButtonDefaultColor;
        StartCoroutine(PausedButtonAlpha());
    }
    public IEnumerator Death()
    {
        waveDisplay.text = "You died!";
        waveDisplay.color = new Color(0.4f, 0, 0, 1);
        StartCoroutine(WaveDisplaySize());
        StartCoroutine(WaveDisplayColor());
        yield return new WaitForSeconds(2f);
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
                yield return new WaitForSeconds(data.timeBetweenDisplayAndSpawn);
                StartCoroutine(SpawnWave(wave));
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator WaveDisplayColor()
    {
        Color waveDisplayColor = waveDisplay.color;
        float alpha = 0;
        for (int i = 0; i < data.waveDisplayAppearingTime*data.targetFPS; i++)
        {
            alpha += 1f / data.waveDisplayAppearingTime / data.targetFPS;
            waveDisplay.color = ChangeAlpha(waveDisplayColor, alpha);
            yield return new WaitForSeconds(1f/data.targetFPS);
        }
        yield return new WaitForSeconds(data.waveDisplayFullAlphaTime);
        for (int i = 0; i < data.waveDisplayDisappearingTime*data.targetFPS; i++)
        {
            alpha -= 1.01f / data.waveDisplayDisappearingTime /data.targetFPS; //at 1f didn't disappear, probably due to rounding
            waveDisplay.color = ChangeAlpha(waveDisplayColor, alpha);
            yield return new WaitForSeconds(1f/data.targetFPS);
        }
    }

    private Color ChangeAlpha(Color color, float value)
    {
        return new Color(color.r, color.g, color.b, value);
    }

    public IEnumerator WaveDisplaySize()
    {
        waveDisplay.fontSize = data.waveDisplayStartFontSize;
        for (int i = 0; i < data.targetFPS*(data.waveDisplayDisappearingTime+data.waveDisplayAppearingTime+data.waveDisplayFullAlphaTime); i++)
        {
            waveDisplay.fontSize += data.waveDisplayFontIncreaseStep;
            yield return new WaitForSeconds(1f/data.targetFPS);
        }
    }

    void UpdateWaveText()
    {
        waveDisplay.text = $"Wave: {wave}";
    }

    IEnumerator SpawnWave(int wave)
    {
        player.GetHealing(0.1f);
        BasicEnemy randomEnemy = null;
        while (randomEnemy == enemyToSpawn || randomEnemy == null)
        {
            randomEnemy = enemiesList[Random.Range(0, enemiesList.Count)];
        }
        enemyToSpawn = randomEnemy;
        int enemiesToSpawn = enemyToSpawn.CalculateEnemiesToSpawn(wave);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            float horizontalMargin = 30;
            float spawnX = Random.Range(-Screen.width/2+horizontalMargin, Screen.width/2-horizontalMargin);
            float spawnY = Screen.height*(0.5f+data.entityBorder);
            BasicEnemy spawnedEnemy = Instantiate(enemyToSpawn, new Vector3(spawnX,spawnY,0), Quaternion.Euler(0,0,0), enemiesParent);
            spawnedEnemy.health = spawnedEnemy.CalculateHealth(wave);
            spawnedEnemy.powerUpPercentChance = spawnedEnemy.health * data.powerUpPercentChancePerHealth;
            spawnedEnemy.speed = spawnedEnemy.CalculateSpeed(wave);
            spawnedEnemy.damage = spawnedEnemy.CalculateDamage(wave);
            spawnedEnemy.gameController = this;
            spawnedEnemy.data = data;
            yield return new WaitForSeconds(spawnedEnemy.CalculateSpawnTime(wave));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public GameController gameController;



    [Header("game state")]
    public bool isAlive;
    public bool isPaused;
    public bool clickedPositionExists;
    [Header("movement settings")]
    public bool usePercentLimits;
    public float entityBorder;
    public float toplimit;
    public float sidelimit;
    public float bottomlimit;
    public float toppxlimit;
    public float sidepxlimit;
    public float bottompxlimit;
    public float followMovementPerSec;
    [Header("balance settings")]
    public float damage;
    public float bulletSpeed;
    public float maxHealth;
    public float health;
    public float attackSpeed;
    public float healthRegenPerWave;
    [Header("ui/player feedback settings")]
    public long vibrationDuration;
    public float waveDisplayAppearingTime;
    public float waveDisplayDisappearingTime;
    public float waveDisplayFullAlphaTime;
    public float targetFPS;
    public float timeBetweenDisplayAndSpawn;
    public float waveDisplayStartFontSize;
    public float waveDisplayFontIncreaseStep;
    public float redFlashMaxAlpha;
    public Color pauseButtonDefaultColor;
    public Color pauseButtonPausedColor;
    public float pauseButtonPulseTime;
    public int pauseButtonPulseDirection;
    public float pauseButtonTargetAlpha;
    public float pauseButtonWidth;
    public float pauseButtonHeight;
    [Header("powerUps")]
    public float powerUpSpeed;
    public float powerUpPercentChancePerHealth;
    public float attackSpeedPerPowerUp;
    [Header("Scaling")]
    public float scaling;
    [Header("sfx/music")]
    public float sfxShootDefault;
    public float sfxHitDefault;
    public float sfxDestroyedDefault;

    public Dictionary<string, float> floatSettings = new Dictionary<string, float>() { };
    public Dictionary<string, bool> boolSettings = new Dictionary<string, bool>() { };
    List<string> floatList = new List<string>();
    List<string> boolList = new List<string>();

    public Vector3[] previousPositions = new Vector3[6]; 

    private void Awake()
    {
        Scaling();
        toplimit *= Screen.height/2;
        bottomlimit *= Screen.height/2;
        sidelimit *= Screen.height/2*(9f/20f);
        floatSettings.Add("Sensitivity", 1f);
        string[] boolsettings = { "Vibration", "SwipeMovement", "RedFlash", "PlaySfx", "PlayMusic" };
        AddBoolSettings(boolsettings);
    }

    public bool CheckIfOut(Transform transform)
    {
        if(transform.position.y > -Screen.height * (0.5 + entityBorder) && transform.position.y < Screen.height * (0.5 + entityBorder) && transform.position.x > -Screen.height * 9f/20f * (0.5 + entityBorder) && transform.position.x < Screen.height * 9f / 20f * (0.5 + entityBorder))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void Scaling()
    {
        scaling = Screen.height / 2400f;
        bulletSpeed *= scaling;
    }

    private void AddBoolSettings(string [] boolsettings)
    {
        for (int i = 0; i < boolsettings.Length; i++)
        {
            boolSettings.Add(boolsettings[i], true);
        }
    }

    private void Start()
    {
        ListBoolSettings();
        LoadBoolSettings();
        ListFloatSettings();
        LoadFloatSettings();
    }

    private void ListBoolSettings()
    {
        foreach (KeyValuePair<string, bool> entry in boolSettings)
        {
            boolList.Add(entry.Key);
        }
    }

    private void ListFloatSettings()
    {
        foreach (KeyValuePair<string, float> entry in floatSettings)
        {
            floatList.Add(entry.Key);
        }
    }

    private void LoadBoolSettings()
    {
        for (int i = 0; i < boolList.Count; i++)
        {
            if (PlayerPrefs.HasKey(boolList[i]))
            {
                boolSettings[boolList[i]] = Methods.IntToBool(PlayerPrefs.GetInt(boolList[i]));
            }
        }
    }

    private void LoadFloatSettings()
    {
        for (int i = 0; i < floatList.Count; i++)
        {
            if (PlayerPrefs.HasKey(floatList[i]))
            {
                floatSettings[floatList[i]] = PlayerPrefs.GetFloat(floatList[i]);
            }
        }
    }
}

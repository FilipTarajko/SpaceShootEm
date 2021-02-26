using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public GameController gameController;



    [Header("game state")]
    public bool isAlive;
    [Header("movement settings")]
    public float entityBorder;
    public float toplimit;
    public float sidelimit;
    public float bottomlimit;
    public float followMovementPerSec;
    [Header("balance settings")]
    public float damage;
    public float bulletSpeed;
    public float maxHealth;
    public float health;
    public float attackSpeed;
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

    public Dictionary<string, float> floatSettings = new Dictionary<string, float>() { };
    public Dictionary<string, bool> boolSettings = new Dictionary<string, bool>() { };
    List<string> floatList = new List<string>();
    List<string> boolList = new List<string>();

    private void Awake()
    {
        toplimit *= Screen.height/2;
        bottomlimit *= Screen.height/2;
        sidelimit *= Screen.width/2;
        floatSettings.Add("Sensitivity", 1f);
        string[] boolsettings = { "Vibration", "SwipeMovement", "RedFlash" };
        AddBoolSettings(boolsettings);
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

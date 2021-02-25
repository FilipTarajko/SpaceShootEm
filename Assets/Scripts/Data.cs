using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public GameController gameController;



    [Header("game state")]
    public bool isAlive;
    [Header("dev settings")]
    public float entityBorder;
    [Header("game design settings")]
    public double damage;
    public float bulletSpeed;
    public float maxHealth;
    public float health;
    [Header("ui/player feedback settings")]
    public long vibrationDuration;

    public Dictionary<string, float> floatSettings = new Dictionary<string, float>() { };
    public Dictionary<string, bool> boolSettings = new Dictionary<string, bool>() { };
    List<string> floatList = new List<string>();
    List<string> boolList = new List<string>();

    private void Awake()
    {
        floatSettings.Add("Sensitivity", 1f);

        boolSettings.Add("Vibration", false );
        boolSettings.Add("SwipeMovement", false);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public GameController gameController;
    public float sensitivity;


    public bool isAlive;
    public bool isVibration;
    [Header("dev settings")]
    public float entityBorder;

    public bool IntToBool(int value)
    {
        if (value == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public int BoolToInt(bool value)
    {
        if (value == true)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            sensitivity = PlayerPrefs.GetFloat("Sensitivity");
        }
        else
        {
            sensitivity = 1;
        }
        if (PlayerPrefs.HasKey("Vibration"))
        {
            isVibration = IntToBool(PlayerPrefs.GetInt("Vibration"));
        }
        else
        {
            isVibration = false;
        }
    }
}

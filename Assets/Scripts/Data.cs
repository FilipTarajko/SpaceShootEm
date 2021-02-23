using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public GameController gameController;
    public float sensitivity;

    [Header("dev settings")]
    public float entityBorder;

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
    }
}

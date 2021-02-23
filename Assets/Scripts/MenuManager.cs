using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] TMP_Text lastScore;
    [SerializeField] TMP_Text highScore;
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text sensitivityText;
    [SerializeField] float sliderDefaultValue;

    void Start()
    {
        StartMenu();
    }
    private void StartMenu()
    {
        InitializeSensitivity();
        HandleText(lastScore, "Last score");
        HandleText(highScore, "Highscore");
        HandleText(sensitivityText, "Sensitivity");
    }

    private void HandleText(TMP_Text tmptext, string key)
    {
        if (key.Equals("Sensitivity"))
        {
            tmptext.text = $"{key}: {PlayerPrefs.GetFloat(key)}";
        }
        else if (PlayerPrefs.HasKey(key))
        {
            tmptext.text = $"{key}: {PlayerPrefs.GetInt(key)}";
        }
        else
        {
            tmptext.text = "";
        }
    }
    private void InitializeSensitivity()
    {
        if (!PlayerPrefs.HasKey("Sensitivity"))
        {
            SensitivitySliderSet();
        }
        else
        {
            slider.value = PlayerPrefs.GetFloat("Sensitivity");
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ResetAllPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        slider.value = sliderDefaultValue;
        StartMenu();
    }

    public void SensitivitySliderSet()
    {
        SetSensitivity((float)slider.value);
    }

    public void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("Sensitivity", (float)System.Math.Round(sensitivity,2));
        PlayerPrefs.Save();
        HandleText(sensitivityText, "Sensitivity");
    }
}

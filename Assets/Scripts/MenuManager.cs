using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Methods methods;
    [SerializeField] TMP_Text lastScore;
    [SerializeField] TMP_Text highScore;
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text sensitivityText;
    [SerializeField] float sliderDefaultValue;

    [SerializeField] TMP_Text authorTextShadow;
    [SerializeField] TMP_Text titleTextShadow;
    public float colorChangeSpeed;
    private float H;
    private float S;
    private float V;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] Toggle vibrationToggle;

    public void ChangeMenu(GameObject targetMenu)
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        targetMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void Start()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        StartMenu();
    }

    private void StartMenu()
    {
        InitializeSensitivity();
        HandleText(lastScore, "Last score");
        HandleText(highScore, "Highscore");
        HandleText(sensitivityText, "Sensitivity");
        HandleToggle(vibrationToggle, "Vibration");
    }

    private void HandleToggle(Toggle toggle, string key)
    {
        if (key.Equals("Vibration"))
        {
            vibrationToggle.isOn = methods.IntToBool(PlayerPrefs.GetInt("Vibration"));
            toggle.onValueChanged.AddListener(delegate { ToggleSetting(toggle.isOn, key); });
        }
    }

    private void ToggleSetting(bool value, string key)
    {
        PlayerPrefs.SetInt(key, methods.BoolToInt(value));
        PlayerPrefs.Save();
    }

    private void HandleText(TMP_Text tmptext, string key)
    {
        if (key.Equals("Sensitivity"))
        {
            tmptext.text = $"{key}: {PlayerPrefs.GetFloat(key):F2}";
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
    
    private void Update()
    {
        ChangeColors();
    }

    void ChangeColors()
    {
        Color.RGBToHSV(authorTextShadow.color, out H, out S, out V);
        authorTextShadow.color = Color.HSVToRGB((H + colorChangeSpeed * Time.deltaTime) % 1, S, V);
        titleTextShadow.color = authorTextShadow.color;
    }
}

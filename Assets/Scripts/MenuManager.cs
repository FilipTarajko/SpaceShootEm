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
    [SerializeField] TMP_Text resetButtonText;
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
    [SerializeField] Toggle swipeMovementToggle;
    [SerializeField] Toggle redFlashToggle;
    [SerializeField] int resetButtonClicks;
    [SerializeField] int clicksToReset;
    private string initialResetButtonText;
    private float initialResetButtonFontSize;
    [SerializeField] RectTransform UiContainer;

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
        //UiContainer.sizeDelta = new Vector2(Screen.height,Screen.height / 2 * (9f / 20f));
        initialResetButtonText = resetButtonText.text;
        initialResetButtonFontSize = resetButtonText.fontSize;
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        StartMenu();
    }

    private void StartMenu()
    {
        resetButtonClicks = 0;
        InitializeSensitivitySlider();
        HandleText(lastScore, "Last score");
        HandleText(highScore, "Highscore");
        HandleText(sensitivityText, "Sensitivity");
        HandleToggle(vibrationToggle, "Vibration");
        HandleToggle(swipeMovementToggle, "SwipeMovement");
        HandleToggle(redFlashToggle, "RedFlash");
    }

    private void HandleToggle(Toggle toggle, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            toggle.isOn = Methods.IntToBool(PlayerPrefs.GetInt(key));
        }
        else
        {
            toggle.isOn = true;
        }
        toggle.onValueChanged.AddListener(delegate { ToggleSetting(toggle.isOn, key); });
    }

    private void ToggleSetting(bool value, string key)
    {
        PlayerPrefs.SetInt(key, Methods.BoolToInt(value));
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
    private void InitializeSensitivitySlider()
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
        if (resetButtonClicks == clicksToReset-1)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            slider.value = sliderDefaultValue;
            StartMenu();
            resetButtonText.text = initialResetButtonText;
            resetButtonText.fontSize = initialResetButtonFontSize;
        }
        else
        {
            resetButtonText.fontSize = 40;
            resetButtonClicks += 1;
            resetButtonText.text = $"Are you sure? Tap { clicksToReset - resetButtonClicks } more times to reset.";
        }
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

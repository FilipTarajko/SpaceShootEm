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
    [Header("Sensitivity")]
    [SerializeField] Slider sensitivitySlider;
    [SerializeField] GameObject sensitivityParent;
    [SerializeField] TMP_Text sensitivityText;
    [SerializeField] TMP_Text resetButtonText;
    [SerializeField] float sensitivitySliderDefaultValue;

    [SerializeField] TMP_Text authorTextShadow;
    [SerializeField] TMP_Text titleTextShadow;
    public float colorChangeSpeed;
    private float H;
    private float S;
    private float V;
    [Header("Menus")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject aboutMenu;
    [Header("Toggles")]
    [SerializeField] Toggle vibrationToggle;
    [SerializeField] Toggle swipeMovementToggle;
    [SerializeField] Toggle redFlashToggle;
    [SerializeField] Toggle playAudioToggle;
    [SerializeField] Toggle playMusicToggle;
    [Header("Reset")]
    [SerializeField] int resetButtonClicks;
    [SerializeField] int clicksToReset;
    private string initialResetButtonText;
    private float initialResetButtonFontSize;
    [Header("")]
    [SerializeField] RectTransform UiContainer;
    [SerializeField] AudioSource audioSourceMenuMusic;
    [SerializeField] float musicVolume;

    public class SliderSetting
    {
        public GameObject parent;
        public Slider slider;
        public string conditionKey;
        public bool conditionRequiredValue;
        public TMP_Text textDisplay;
        public float defaultValue;
    }

    private Dictionary<string, SliderSetting> sliderSettings = new Dictionary<string, SliderSetting>() { };

    public void ChangeMenu(GameObject targetMenu)
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        aboutMenu.SetActive(false);
        targetMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void Start()
    {
        sliderSettings.Add("Sensitivity", new SliderSetting() { parent = sensitivityParent, slider = sensitivitySlider, conditionKey = "SwipeMovement", conditionRequiredValue = true, textDisplay = sensitivityText, defaultValue = sensitivitySliderDefaultValue });
        audioSourceMenuMusic = MusicAudioSource.Instance.gameObject.GetComponent<AudioSource>(); ;
        //UiContainer.sizeDelta = new Vector2(Screen.height,Screen.height / 2 * (9f / 20f));
        initialResetButtonText = resetButtonText.text;
        initialResetButtonFontSize = resetButtonText.fontSize;
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        aboutMenu.SetActive(false);
        StartMenu();
    }

    private void StartMenu()
    {
        resetButtonClicks = 0;
        HandleText(lastScore, "Last score");
        HandleText(highScore, "Highscore");
        HandleText(sensitivityText, "Sensitivity");
        HandleToggle(vibrationToggle, "Vibration");
        HandleToggle(swipeMovementToggle, "SwipeMovement");
        HandleToggle(redFlashToggle, "RedFlash");
        HandleToggle(playAudioToggle, "PlaySfx");
        HandleToggle(playMusicToggle, "PlayMusic");
        SetMusicVolume();
        SetSliderVisibility("Sensitivity");
        playMusicToggle.onValueChanged.AddListener(delegate { SetMusicVolume(); });
        swipeMovementToggle.onValueChanged.AddListener(delegate { SetSliderVisibility("Sensitivity"); });
    }

    private void SetMusicVolume()
    {
        if (PlayerPrefs.HasKey("PlayMusic"))
        {
            if (Methods.IntToBool(PlayerPrefs.GetInt("PlayMusic")))
            {
                audioSourceMenuMusic.volume = musicVolume;
            }
            else
            {
                audioSourceMenuMusic.volume = 0;
            }
        }
        else
        {
            audioSourceMenuMusic.volume = musicVolume;
        }
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
        if (PlayerPrefs.HasKey(key))
        {
            tmptext.text = $"{key}: {PlayerPrefs.GetInt(key)}";
        }
        else
        {
            tmptext.text = "";
        }
    }
    private void SetSliderVisibility(string setting)
    {
        bool toSetVisible = false;
        if (!PlayerPrefs.HasKey(sliderSettings[setting].conditionKey))
        {
            toSetVisible = sliderSettings[setting].conditionRequiredValue;
        }
        else if (Methods.IntToBool(PlayerPrefs.GetInt(sliderSettings[setting].conditionKey)) == sliderSettings[setting].conditionRequiredValue)
        {
            toSetVisible = true;
        }
        if (toSetVisible)
        {
            SetSlider(setting);
        }
        else
        {
            sensitivityParent.gameObject.SetActive(false);
        }
    }

    private void SetSlider(string setting)
    {
        sensitivityParent.gameObject.SetActive(true);
        if (PlayerPrefs.HasKey(setting))
        {
            SetSliderValueFromPrefs(setting);
        }
        else
        {
            sensitivitySlider.value = sensitivitySliderDefaultValue;
            SetFloatSetting(setting);
        }
    }

    private void SetSliderValueFromPrefs(string setting)
    {
        sliderSettings[setting].slider.value = PlayerPrefs.GetFloat(setting);
    }

    public void SetFloatSetting(string setting)
    {
        PlayerPrefs.SetFloat(setting, (float)System.Math.Round(sliderSettings[setting].slider.value, 2));
        PlayerPrefs.Save();
        HandleFloatDisplay(setting);
    }
    private void HandleFloatDisplay(string setting)
    {
        sliderSettings[setting].textDisplay.text = $"{setting}: {PlayerPrefs.GetFloat(setting):F2}";
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

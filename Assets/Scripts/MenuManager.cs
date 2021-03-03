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
    [SerializeField] TMP_Text authorTextShadow;
    [SerializeField] TMP_Text titleTextShadow;
    public float colorChangeSpeed;
    private float H;
    private float S;
    private float V;
    [Header("Sensitivity")]
    [SerializeField] Slider sensitivitySlider;
    [SerializeField] GameObject sensitivityParent;
    [SerializeField] TMP_Text sensitivityText;
    [SerializeField] float sensitivitySliderDefaultValue;
    [Header("SfxVolume")]
    [SerializeField] Slider sfxSlider;
    [SerializeField] GameObject sfxParent;
    [SerializeField] TMP_Text sfxText;
    [SerializeField] float sfxSliderDefaultValue;
    [Header("MusicVolume")]
    [SerializeField] Slider musicSlider;
    [SerializeField] GameObject musicParent;
    [SerializeField] TMP_Text musicText;
    [SerializeField] float musicSliderDefaultValue;
    [Header("Menus")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject aboutMenu;
    [Header("Reset")]
    [SerializeField] TMP_Text resetButtonText;
    [SerializeField] int resetButtonClicks;
    [SerializeField] int clicksToReset;
    private string initialResetButtonText;
    private float initialResetButtonFontSize;
    [Header("")]
    [SerializeField] RectTransform UiContainer;
    [SerializeField] AudioSource audioSourceMenuMusic;
    [SerializeField] float musicVolume;
    [SerializeField] SettingToggle settingTogglePrefab;
    [SerializeField] Transform settingsContent;

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
    private Dictionary<string, SettingToggle> toggles = new Dictionary<string, SettingToggle>() { };

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

    //void GenerateSettingBars()
    //{
    //    foreach (KeyValuePair<string, bool> entry in data.settings)
    //   {
    //       SettingBar setting = Instantiate(settingBar, SettingsContent);
    //       setting.gameController = this;
    //      setting.settingName = entry.Key;
    //       if (PlayerPrefs.HasKey(entry.Key))
    //      {
    //          if (!IntToBool(PlayerPrefs.GetInt(entry.Key)))
    //          {
    //              setting.ignore = 1;
    //              setting.GetComponentInChildren<Toggle>().isOn = false;
    //          }
    //      }
    //   }
    // }

    void GenerateSettingToggles()
    {
        string[] boolsettings = { "Vibration", "SwipeMovement", "RedFlash", "PlaySfx", "PlayMusic" };
        for (int i = 0; i < boolsettings.Length; i++ )
        {
            SettingToggle setting = Instantiate(settingTogglePrefab, settingsContent);
            toggles.Add(boolsettings[i], setting);
            setting.text.text = boolsettings[i];
            print($"{setting.toggle}, {boolsettings[i]}");
            HandleToggle(setting.toggle, boolsettings[i]);
        }
    }

    void Start()
    {
        sliderSettings.Add("Sensitivity", new SliderSetting() { parent = sensitivityParent, slider = sensitivitySlider, conditionKey = "SwipeMovement", conditionRequiredValue = true, textDisplay = sensitivityText, defaultValue = sensitivitySliderDefaultValue });
        sliderSettings.Add("SfxVolume", new SliderSetting() { parent = sfxParent, slider = sfxSlider, conditionKey = "PlaySfx", conditionRequiredValue = true, textDisplay = sfxText, defaultValue = sfxSliderDefaultValue });
        sliderSettings.Add("MusicVolume", new SliderSetting() { parent = musicParent, slider = musicSlider, conditionKey = "PlayMusic", conditionRequiredValue = true, textDisplay = musicText, defaultValue = musicSliderDefaultValue });
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
        GenerateSettingToggles();
        resetButtonClicks = 0;
        HandleText(lastScore, "Last score");
        HandleText(highScore, "Highscore");
        HandleText(sensitivityText, "Sensitivity");
        HandleText(sfxText, "SfxVolume");
        HandleText(musicText, "MusicVolume");
        SetSliderVisibility("Sensitivity");
        SetSliderVisibility("SfxVolume");
        SetSliderVisibility("MusicVolume");
        SetMusicVolume();
        toggles["PlayMusic"].toggle.onValueChanged.AddListener(delegate { SetMusicVolume(); });
        toggles["SwipeMovement"].toggle.onValueChanged.AddListener(delegate { SetSliderVisibility("Sensitivity"); });
        toggles["PlaySfx"].toggle.onValueChanged.AddListener(delegate { SetSliderVisibility("SfxVolume"); });
        toggles["PlayMusic"].toggle.onValueChanged.AddListener(delegate { SetSliderVisibility("MusicVolume"); });
    }

    public void SetMusicVolume()
    {
        if (PlayerPrefs.HasKey("PlayMusic"))
        {
            if (Methods.IntToBool(PlayerPrefs.GetInt("PlayMusic")))
            {
                audioSourceMenuMusic.volume = musicVolume * (PlayerPrefs.GetFloat("MusicVolume"));
            }
            else
            {
                audioSourceMenuMusic.volume = 0;
            }
        }
        else
        {
            audioSourceMenuMusic.volume = musicVolume * (PlayerPrefs.GetFloat("MusicVolume"));
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
            sliderSettings[setting].parent.SetActive(false);
        }
    }

    private void SetSlider(string setting)
    {
        sliderSettings[setting].parent.SetActive(true);
        if (PlayerPrefs.HasKey(setting))
        {
            SetSliderValueFromPrefs(setting);
        }
        else
        {
            sliderSettings[setting].slider.value = sliderSettings[setting].defaultValue;
        }
        SetFloatSetting(setting);
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

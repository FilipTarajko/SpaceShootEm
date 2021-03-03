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
    [Header("Settings default values")]
    [SerializeField] float sensitivitySliderDefaultValue;
    [SerializeField] float sfxSliderDefaultValue;
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
    [SerializeField] SettingSlider settingSliderPrefab;
    [SerializeField] Transform settingsContent;

    private Dictionary<string, SettingSlider> sliders = new Dictionary<string, SettingSlider>() { };
    private Dictionary<string, SliderSetting> sliderSettings = new Dictionary<string, SliderSetting>() { };
    private Dictionary<string, SettingToggle> toggles = new Dictionary<string, SettingToggle>() { };

    public class SliderSetting
    {
        public string conditionKey;
        public bool conditionRequiredValue;
        public float defaultValue;
        public float minValue;
        public float maxValue;
    }


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

    void GenerateSettingToggles()
    {
        sliderSettings.Add("Sensitivity", new SliderSetting() { conditionKey = "SwipeMovement", conditionRequiredValue = true, defaultValue = sensitivitySliderDefaultValue, minValue = 0.2f, maxValue = 5 });
        sliderSettings.Add("SfxVolume", new SliderSetting() { conditionKey = "PlaySfx", conditionRequiredValue = true, defaultValue = sfxSliderDefaultValue, minValue = 0.01f, maxValue = 1 });
        sliderSettings.Add("MusicVolume", new SliderSetting() { conditionKey = "PlayMusic", conditionRequiredValue = true, defaultValue = musicSliderDefaultValue, minValue = 0.01f, maxValue = 1 });
        sliderSettings.Add("Red", new SliderSetting() { conditionKey = "CustomPlayerColor", conditionRequiredValue = true, defaultValue = 1, minValue = 0, maxValue = 1 });
        sliderSettings.Add("Green", new SliderSetting() { conditionKey = "CustomPlayerColor", conditionRequiredValue = true, defaultValue = 1, minValue = 0, maxValue = 1 });
        sliderSettings.Add("Blue", new SliderSetting() { conditionKey = "CustomPlayerColor", conditionRequiredValue = true, defaultValue = 1, minValue = 0, maxValue = 1 });
        string[] boolsettings = { "Vibration", "SwipeMovement", "RedFlash", "PlaySfx", "PlayMusic", "CustomPlayerColor" };
        for (int i = 0; i < boolsettings.Length; i++ )
        {
            SettingToggle setting = Instantiate(settingTogglePrefab, settingsContent);
            toggles.Add(boolsettings[i], setting);
            setting.text.text = boolsettings[i];
            HandleToggle(setting.toggle, boolsettings[i]);
            foreach (KeyValuePair<string, SliderSetting> entry in sliderSettings)
            {
                if(entry.Value.conditionKey == boolsettings[i])
                {
                    GenerateSettingSlider(entry.Key);
                    toggles[boolsettings[i]].toggle.onValueChanged.AddListener(delegate { SetSliderVisibility(entry.Key); });
                }
            }
        }
    }

    void GenerateSettingSlider(string chosenSlider)
    {
        SettingSlider slider = Instantiate(settingSliderPrefab, settingsContent);
        sliders.Add(chosenSlider, slider);
        slider.textDisplay.text = chosenSlider;
        slider.conditionRequiredValue = sliderSettings[chosenSlider].conditionRequiredValue;
        slider.conditionKey = sliderSettings[chosenSlider].conditionKey;
        slider.slider.minValue = sliderSettings[chosenSlider].minValue;
        slider.slider.maxValue = sliderSettings[chosenSlider].maxValue;
        slider.defaultValue = sliderSettings[chosenSlider].defaultValue;
        HandleText(slider.textDisplay, chosenSlider);
        slider.slider.onValueChanged.AddListener(delegate { SetFloatSetting(chosenSlider); });
        if (chosenSlider == "MusicVolume")
        {
            slider.slider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
        }
        SetSliderVisibility(chosenSlider);
    }

    void Start()
    {
        audioSourceMenuMusic = MusicAudioSource.Instance.gameObject.GetComponent<AudioSource>(); ;
        //UiContainer.sizeDelta = new Vector2(Screen.height,Screen.height / 2 * (9f / 20f));
        initialResetButtonText = resetButtonText.text;
        initialResetButtonFontSize = resetButtonText.fontSize;
        StartMenu();
    }

    private void StartMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        aboutMenu.SetActive(false);
        GenerateSettingToggles();
        resetButtonClicks = 0;
        HandleText(lastScore, "Last score");
        HandleText(highScore, "Highscore");
        SetMusicVolume();
        toggles["PlayMusic"].toggle.onValueChanged.AddListener(delegate { SetMusicVolume(); });
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
        toggle.onValueChanged.AddListener(delegate { ChangeToggleSetting(toggle.isOn, key); });
    }

    private void ChangeToggleSetting(bool value, string key)
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
        if (!PlayerPrefs.HasKey(sliders[setting].conditionKey))
        {
            toSetVisible = sliders[setting].conditionRequiredValue;
        }
        else if (Methods.IntToBool(PlayerPrefs.GetInt(sliders[setting].conditionKey)) == sliders[setting].conditionRequiredValue)
        {
            toSetVisible = true;
        }
        if (toSetVisible)
        {
            SetSlider(setting);
        }
        else
        {
            sliders[setting].parent.SetActive(false);
        }
    }

    private void SetSlider(string setting)
    {
        sliders[setting].parent.SetActive(true);
        if (PlayerPrefs.HasKey(setting))
        {
            SetSliderValueFromPrefs(setting);
        }
        else
        {
            sliders[setting].slider.value = sliders[setting].defaultValue;
        }
        SetFloatSetting(setting);
    }

    private void SetSliderValueFromPrefs(string setting)
    {
        sliders[setting].slider.value = PlayerPrefs.GetFloat(setting);
    }

    public void SetFloatSetting(string setting)
    {
        PlayerPrefs.SetFloat(setting, (float)System.Math.Round(sliders[setting].slider.value, 2));
        PlayerPrefs.Save();
        HandleFloatDisplay(setting);
    }
    private void HandleFloatDisplay(string setting)
    {
        sliders[setting].textDisplay.text = $"{setting}: {PlayerPrefs.GetFloat(setting):F2}";
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
            SceneManager.LoadScene(0);
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

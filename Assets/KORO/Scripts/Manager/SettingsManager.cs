using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider  _musicVolumeSlider;
    [SerializeField] private Slider  _sfxVolumeSlider;
    [SerializeField] private AudioMixer  _muusicMixer;
    [SerializeField] private float  _sfxVolume;

    [Header("Graphic Settings")]
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Dropdown qualityDropdown;
    public Toggle fullscreenToggle;

    [Header("Game Settings")]
    [SerializeField] private Dropdown languageDropdown;
    [SerializeField] private GameObject tickObject;
    public Toggle tutorialToggle;
    private Resolution[] availableResolutions;
    private PlayerPrefsManager _playerPrefsManager;
    public GameObject settingsPanel;

    // Start is called before the first frame update
    
    private readonly Resolution[] customResolutions = new Resolution[]
    {
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1280, height = 720 },
        new Resolution { width = 1600, height = 900 }
    };
    private readonly int[] customQualityIndices = { 0,1,2,3,4 }; // Low, Medium, High
    private readonly string[] qualityString = { "Key_VeryLow","Key_Low","Key_Medium","Key_High","Key_VeryHigh" }; // Low, Medium, High
    private readonly Enums.LanguageType[] languageIndices = { Enums.LanguageType.English, Enums.LanguageType.Spanish, Enums.LanguageType.Turkish,Enums.LanguageType.Chinesee }; // Low, Medium, High
    [SerializeField] private LocalizedStringTable localizedStringTable;
    
    void Start()
    {
        _playerPrefsManager = PlayerPrefsManager.Instance;
        //availableResolutions = Screen.resolutions;
        InitiliazeFullScreen();
        InitiliazeSliders();
        InitiliazeResolution();
        InitiliazeLanguage();
        InitiliazeQuality();
        
    }

    public void InitiliazeSliders()
    {
        _sfxVolumeSlider.value = _playerPrefsManager.LoadVolume();
    }
    public void InitiliazeResolution()
    {
        availableResolutions = customResolutions;
        resolutionDropdown.ClearOptions(); 

        int currentResolutionIndex = 0;
        var options = new System.Collections.Generic.List<string>();
        for (int i = 0; i < availableResolutions.Length; i++)
        {
            string option = availableResolutions[i].width + " x " + availableResolutions[i].height;
            options.Add(option);

            if (availableResolutions[i].width == Screen.currentResolution.width &&
                availableResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options); 
        resolutionDropdown.value = currentResolutionIndex; 
        resolutionDropdown.RefreshShownValue();
        var resolution = _playerPrefsManager.LoadResolution();
        SetResolution(resolution);
        resolutionDropdown.value = resolution;
    }

    public void InitiliazeQuality()
    {
        var stringTable = localizedStringTable.GetTable();
        qualityDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();
        foreach (var index in customQualityIndices)
        {
            //options.Add(QualitySettings.names[index]);

            var localizedString = stringTable.GetEntry(qualityString[index]).GetLocalizedString();
            options.Add(localizedString);
        }
        qualityDropdown.AddOptions(options);
        
        int currentQuality = QualitySettings.GetQualityLevel();
        int dropdownIndex = System.Array.IndexOf(customQualityIndices, currentQuality);
        qualityDropdown.value = dropdownIndex >= 0 ? dropdownIndex : 0; // Eğer mevcut kalite custom listede değilse ilk kaliteyi seç
        qualityDropdown.RefreshShownValue();
        
        var quality = _playerPrefsManager.LoadQuality();
        SetQuality(quality);
        qualityDropdown.value = quality;
    }
    public void InitiliazeLanguage()
    {
        languageDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();
        foreach (var index in languageIndices)
        {
            options.Add(index.ToString());
        }

        languageDropdown.AddOptions(options);
        
        if (_playerPrefsManager.GetBool("Initiliazed"))
        {
            var languageIndex = PlayerPrefsManager.Instance.LoadLanguage();
            languageDropdown.value = languageIndex;
        }
        else
        {
            var languageIndex = LocalizationSettings.SelectedLocale;
            languageDropdown.value = languageIndex.SortOrder;
        }
        
    }
    
    
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void ChangeSliderValue()
    {
        _sfxVolume = _sfxVolumeSlider.value;
        _muusicMixer.SetFloat("SFX", Mathf.Log10(_sfxVolume)*20);
    }

    public void SetResolution()
    {
        SetResolution(resolutionDropdown.value);
    }
    public void SetLanguage()
    {
        SetLanguageWithIndex(languageDropdown.value);
        InitiliazeQuality();
    }

    public void SetQualityNoIndex()
    {
        SetQuality(qualityDropdown.value);
    }
    public void SetQuality(int index)
    {
        int qualityIndex = customQualityIndices[index];
        QualitySettings.SetQualityLevel(qualityIndex);
        _playerPrefsManager.SaveQuality(index);
    }
    public void EndDragedd()
    {
        _playerPrefsManager.SaveVolume(_sfxVolume);
    }
    public void SetResolution(int resolutionIndex)
    {
        // Seçilen çözünürlüğü uygula
        Resolution resolution = availableResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        _playerPrefsManager.SaveResolution(resolutionIndex);
    }
    public void SetLanguageWithIndex(int languageIndex)
    {
        _playerPrefsManager.SaveLanguage(languageIndex);

        Locale localeLanguage = null;
        
        LocalizationSettings.SelectedLocale = GetLocation();
    }

    public Locale GetLocation()
    {
        var selectedLocale = new Locale();
        switch (_playerPrefsManager.LoadLanguage())
        {
            case 0:
                selectedLocale = LocalizationSettings.AvailableLocales.GetLocale("en");
                break;
            case 1:
                selectedLocale = LocalizationSettings.AvailableLocales.GetLocale("es");
                break;
            case 2:
                selectedLocale = LocalizationSettings.AvailableLocales.GetLocale("tr-TR");
                break;
            case 3:
                selectedLocale = LocalizationSettings.AvailableLocales.GetLocale("zh");
                break;
        }

        return selectedLocale;
    }

    public void SetFullscreen()
    {
        // Tam ekran/pencere modu değiştir
        var isFullScreen = Screen.fullScreen;
        if (isFullScreen)
        {
            _playerPrefsManager.SaveFullScreen(0);
            tickObject.SetActive(false);

        }
        else
        {
            _playerPrefsManager.SaveFullScreen(1);
            tickObject.SetActive(true);

        }
        Screen.fullScreen = !isFullScreen;
    }

    public void InitiliazeFullScreen()
    {
        if (_playerPrefsManager.LoadFullScreen() == 0)
        {
            Screen.fullScreen = false;
            tickObject.SetActive(false);
        }
        else
        {
            Screen.fullScreen = true;
            tickObject.SetActive(true);

        }
    }
}

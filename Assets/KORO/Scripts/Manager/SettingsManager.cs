using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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
    public Dropdown languageDropdown;
    public Toggle tutorialToggle;
    private Resolution[] availableResolutions;    
    public GameObject settingsPanel;
    // Start is called before the first frame update
    
    private readonly Resolution[] customResolutions = new Resolution[]
    {
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1280, height = 720 },
        new Resolution { width = 1600, height = 900 }
    };
    private readonly int[] customQualityIndices = { 0, 2, 4 }; // Low, Medium, High
    void Start()
    {
        //availableResolutions = Screen.resolutions;
        InitiliazeSliders();
        InitiliazeResolution();
        
        InitiliazeQuality();
        
    }

    public void InitiliazeSliders()
    {
        _sfxVolumeSlider.value = PlayerPrefsManager.Instance.LoadVolume();
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
        var resolution = PlayerPrefsManager.Instance.LoadResolution();
        SetResolution(resolution);
        resolutionDropdown.value = resolution;
    }

    public void InitiliazeQuality()
    {
        qualityDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();
        foreach (var index in customQualityIndices)
        {
            options.Add(QualitySettings.names[index]);
        }

        qualityDropdown.AddOptions(options);

        int currentQuality = QualitySettings.GetQualityLevel();
        int dropdownIndex = System.Array.IndexOf(customQualityIndices, currentQuality);
        qualityDropdown.value = dropdownIndex >= 0 ? dropdownIndex : 0; // Eğer mevcut kalite custom listede değilse ilk kaliteyi seç
        qualityDropdown.RefreshShownValue();
        
        var quality = PlayerPrefsManager.Instance.LoadQuality();
        SetQuality(quality);
        qualityDropdown.value = quality;
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

    public void SetQualityNoIndex()
    {
        SetQuality(qualityDropdown.value);
    }
    public void SetQuality(int index)
    {
        int qualityIndex = customQualityIndices[index];
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefsManager.Instance.SaveQuality(index);
    }
    public void EndDragedd()
    {
        PlayerPrefsManager.Instance.SaveVolume(_sfxVolume);
    }
    public void SetResolution(int resolutionIndex)
    {
        // Seçilen çözünürlüğü uygula
        Resolution resolution = availableResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefsManager.Instance.SaveResolution(resolutionIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        // Tam ekran/pencere modu değiştir
        Screen.fullScreen = isFullscreen;
    }
}

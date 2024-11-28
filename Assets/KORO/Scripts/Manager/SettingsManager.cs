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
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public Toggle fullscreenToggle;

    [Header("Game Settings")]
    public Dropdown languageDropdown;
    public Toggle tutorialToggle;
    private Resolution[] availableResolutions;    
    public GameObject settingsPanel;
    // Start is called before the first frame update
    void Start()
    {
        availableResolutions = Screen.resolutions;
        _sfxVolumeSlider.value = PlayerPrefsManager.Instance.LoadVolume();
        
        
        InitiliazeResolution();
        
    }

    public void InitiliazeResolution()
    {
        availableResolutions = Screen.resolutions;
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

    public void EndDragedd()
    {
        PlayerPrefsManager.Instance.SaveVolume(_sfxVolume);
    }
}

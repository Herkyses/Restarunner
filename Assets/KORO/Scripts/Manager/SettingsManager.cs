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
    
    public GameObject settingsPanel;
    // Start is called before the first frame update
    void Start()
    {
        _sfxVolumeSlider.value = PlayerPrefsManager.Instance.LoadVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
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

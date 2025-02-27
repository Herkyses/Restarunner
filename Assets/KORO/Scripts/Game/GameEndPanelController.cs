using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndPanelController : MonoBehaviour
{
    public static GameEndPanelController Instance;

    public TextMeshProUGUI TotalCustomerText;
    public TextMeshProUGUI GainedCash;
    public TextMeshProUGUI Popularity;
    
    public int GameStartTotalCustomer;
    public float GameStartGainedCash;
    public float GameStartPopularity;

    [SerializeField] private Transform _panelTransform;
    [SerializeField] private Transform _dayFinishedInfoPanel;
    [SerializeField] private Tween _dayFinishedInfoPanelTween;
    
    private PlayerPrefsManager _playerPrefsManager;

    private void OnEnable()
    {
        DayNightCycle.IsNightStarted += DayEndInfoStarted;
    }

    private void OnDisable()
    {
        DayNightCycle.IsNightStarted -= DayEndInfoStarted;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {
        GameSceneCanvas.Instance.CanMove = true;
        GameSceneCanvas.IsCursorVisible?.Invoke(false);
        _playerPrefsManager = PlayerPrefsManager.Instance;
        GameStartTotalCustomer = _playerPrefsManager.LoadCustomerCount();
        GameStartGainedCash = _playerPrefsManager.LoadPlayerMoney();
        GameStartPopularity = _playerPrefsManager.LoadPopularity();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && DayNightCycle.Instance.IsNightBegun)
        {
            SetGameEndTexts();
        }
    }

    private void DayEndInfoStarted()
    {
        _dayFinishedInfoPanel.gameObject.SetActive(true);
        if (_dayFinishedInfoPanelTween != null)
        {
            _dayFinishedInfoPanelTween.Kill();
            _dayFinishedInfoPanelTween = null;
        }
        _dayFinishedInfoPanelTween = _dayFinishedInfoPanel.gameObject.GetComponent<Image>().DOFade(0f, 0.5f).SetLoops(-1);
    }
    public void SetGameEndTexts()
    {
        _panelTransform.gameObject.SetActive(true);
        _dayFinishedInfoPanel.gameObject.SetActive(false);
        TotalCustomerText.text = ((_playerPrefsManager.LoadCustomerCount() - GameStartTotalCustomer)).ToString();
        GainedCash.text = (_playerPrefsManager.LoadPlayerMoney() - GameStartGainedCash).ToString();
        Popularity.text = ((_playerPrefsManager.LoadPopularity() - GameStartPopularity)*5f).ToString();
        //Time.timeScale = 0f;         
        Cursor.visible = true;       
        Cursor.lockState = CursorLockMode.None; 
        
        DayEnded();

    }
    public void DayEnded()
    {
        SceneManager.LoadScene("SampleScene");
        Cursor.visible = true;       
        Cursor.lockState = CursorLockMode.None;
        
    }
    public void CloseButtonPressed()
    {
        //Time.timeScale = 1f;          
        Cursor.visible = false;       
        Cursor.lockState = CursorLockMode.Locked; 
        GameSceneCanvas.Instance.CanMove = true;
        if (_dayFinishedInfoPanelTween != null)
        {
            _dayFinishedInfoPanelTween.Kill();
            _dayFinishedInfoPanelTween = null;
        }
        _panelTransform.gameObject.SetActive(false);
        Start();
    }
    // Update is called once per frame
    
}

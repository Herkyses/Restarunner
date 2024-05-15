using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class GameSceneCanvas : MonoBehaviour
{
    public static GameSceneCanvas Instance;
    [SerializeField] private TextMeshProUGUI _ownedMoneyText;
    [SerializeField] private TextMeshProUGUI _popularityText;
    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField] private GameObject _infoObject;
    public MusicController _musicController;
    public bool CanShowCanvas;

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

    }

    public void UpdateMoneyText(float gain,int popularity)
    {
        _ownedMoneyText.text = gain.ToString("F2");
        _popularityText.text = popularity.ToString();
    }

    public void ShowAreaInfo(string areaInfo)
    {
        if (!_infoObject.activeSelf)
        {
            _infoObject.SetActive(true);
            _infoText.text = areaInfo;
        }
    }
    public void UnShowAreaInfo()
    {
        if (_infoObject.activeSelf)
        {
            _infoObject.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

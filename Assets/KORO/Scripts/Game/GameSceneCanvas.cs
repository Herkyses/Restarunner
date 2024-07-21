using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneCanvas : MonoBehaviour
{
    public static GameSceneCanvas Instance;
    [SerializeField] private TextMeshProUGUI _ownedMoneyText;
    [SerializeField] private TextMeshProUGUI _popularityText;
    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField] private TextMeshProUGUI[] _infoTexts;
    [SerializeField] private TextMeshProUGUI[] _infoButtonsTexts;
    [SerializeField] private GameObject _infoObject;
    [SerializeField] private GameObject _objectInfoTextsParent;
    public OrderPanelController _orderPanel;
    public MusicController _musicController;
    public Image _orderPanelImage;
    public static Action UpdateAISpawnController;
    
    public bool CanShowCanvas;
    public bool CanMove;

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

    private void Start()
    {
        UpdateMoneyText(PlayerPrefsManager.Instance.LoadPlayerMoney());
        UpdatePopularityText(PlayerPrefsManager.Instance.LoadPopularity());

    }

    public void AddPopularity()
    {
        var popularity = PlayerPrefsManager.Instance.LoadPopularity();
        popularity++;
        if (popularity % 2 == 0)
        {
            UpdateAISpawnController?.Invoke();
        }
        PlayerPrefsManager.Instance.SavePopularity(popularity);
        UpdatePopularityText(popularity);
    }

    private void OnEnable()
    {
        PlayerPrefsManager.GainedMoney += UpdateMoneyText;
    }

    private void OnDisable()
    {
        PlayerPrefsManager.GainedMoney -= UpdateMoneyText;

    }

    public void UpdateMoneyText(float gain)
    {
        _ownedMoneyText.text = gain.ToString("F2");
    }
    public void UpdatePopularityText(int popularity)
    {
        _popularityText.text = popularity.ToString();
    }

    public void ShowAreaInfo(string areaInfo)
    {
        if (areaInfo != null)
        {
            if (!_infoObject.activeSelf)
            {
                _infoObject.SetActive(true);
            }
            _infoText.text = areaInfo;
        }
        

    }
    public void ShowAreaInfoForTexts(string[] areaInfo)
    {
        if (areaInfo != null)
        {
            if (!_objectInfoTextsParent.activeSelf)
            {
                _objectInfoTextsParent.SetActive(true);
                
            }
            for (int i = 0; i < _infoTexts.Length; i++)
            {
                if (i < areaInfo.Length)
                {
                    _infoTexts[i].text = areaInfo[i];

                }
                else
                {
                    _infoTexts[i].text = null;
                    _infoButtonsTexts[i].text = null;
                }
                    
            }
            
        }
    }
    public void ShowAreaInfoForTextsButtons(string[] areaInfoButton)
    {
        if (areaInfoButton != null)
        {
            for (int i = 0; i < areaInfoButton.Length; i++)
            {
                _infoButtonsTexts[i].text = areaInfoButton[i];
                    
            }
            
        }
    }
    public void UnShowAreaInfo()
    {
        if (_infoObject.activeSelf)
        {
            _infoObject.SetActive(false);
        }
        if (_objectInfoTextsParent.activeSelf)
        {
            _objectInfoTextsParent.SetActive(false);
            
        }
    }
    

    public void CanFollowTrue()
    {
        _orderPanel.CanFollowTrue();
    }
    
    public void zort()
    {
        MealManager mealManager = new MealManager();

        // Yemekleri yükle ve Burger yap
        mealManager.LoadMeals();

        mealManager.MakeMeal(Enums.OrderType.Burger,1);

        // Veriyi tekrar yükle ve kalan miktarları kontrol et
        MealsList loadedMealsList = PlayerPrefsManager.Instance.LoadMeals();
        if (loadedMealsList != null)
        {
            foreach (Meal meal in loadedMealsList.meals)
            {
                Debug.Log("Meal: " + meal.mealName + ", Ingredient Quantity: " + meal.ingredientQuantity);
            }
        }
        else
        {
            Debug.Log("No data found.");
        }
    }
    
}

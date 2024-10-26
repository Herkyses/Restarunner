using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneCanvas : MonoBehaviour
{
    public static GameSceneCanvas Instance;
    [SerializeField] private TextMeshProUGUI _ownedMoneyText;
    [SerializeField] private TextMeshProUGUI _popularityText;
    [SerializeField] private TextMeshProUGUI _totalCustomerText;
    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField] private TextMeshProUGUI _cleanRateText;
    [SerializeField] private TextMeshProUGUI[] _infoTexts;
    [SerializeField] private TextMeshProUGUI[] _infoButtonsTexts;
    [SerializeField] private GameObject[] _infoButtonsParent;
    [SerializeField] private GameObject _infoObject;
    [SerializeField] private GameObject _objectInfoTextsParent;
    
    public OrderPanelController _orderPanel;
    public CatchNonPayerPanelController _catchNonPayerPanel;
    public MusicController _musicController;
    public Image _orderPanelImage;
    public static Action UpdateAISpawnController;
    
    public bool CanShowCanvas;
    public bool CanMove;
    public bool CheckShowInfoText  = true;

    public static Action<bool> IsCursorVisible;

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
        CustomerCountUpdate(PlayerPrefsManager.Instance.LoadCustomerCount());
        CheckShowInfoText  = true;
    }

    public void AddPopularity()
    {
        var popularity = PlayerPrefsManager.Instance.LoadCustomerCount();
        popularity++;
        if (popularity % 2 == 0)
        {
            UpdateAISpawnController?.Invoke();
        }
        PlayerPrefsManager.Instance.SaveCustomerCount(popularity);
        CustomerCountUpdate(popularity);
    }

    private void OnEnable()
    {
        //PlayerPrefsManager.GainedMoney += UpdateMoneyText;
        PlayerPrefsManager.GainedMoney += AddValue;
        IsCursorVisible += CursorActive;
    }

    private void OnDisable()
    {
        //PlayerPrefsManager.GainedMoney -= UpdateMoneyText;
        PlayerPrefsManager.GainedMoney -= AddValue;
        IsCursorVisible -= CursorActive;

    }

    public void UpdateMoneyText(float gain)
    {
        _ownedMoneyText.text = gain.ToString("F2");
    }
    public void AddValue(float amount,float currentValue)
    {
        
        float newTarget = currentValue + amount;
        
        DOTween.To(() => currentValue, x => 
        {
            currentValue = x;         // Güncel değeri güncelle
            UpdateText(currentValue); // Text'i güncelle
        }, newTarget, 0.3f); // 1 saniyede hedefe ulaşır, süreyi ayarlayabilirsiniz.
    }

    void UpdateText(float value)
    {
        _ownedMoneyText.text = value.ToString("F2"); // Virgülden sonrası için F0 kullanabilirsiniz.
    }
    public void CustomerCountUpdate(float popularity)
    {
        _totalCustomerText.text = ((float)popularity).ToString();
    }

    public void UpdatePopularity(float popularity)
    {
        _popularityText.text = ((float)popularity*5).ToString("F2");

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

    public void CursorActive(bool active)
    {
        Cursor.visible = active;
        if (!active)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;

        }
    }
    public void ShowAreaInfoForTexts(string[] areaInfo)
    {
        if (areaInfo != null)
        {
            //if (!_objectInfoTextsParent.activeSelf)
            //{
                _objectInfoTextsParent.SetActive(true);
                _objectInfoTextsParent.GetComponent<RectTransform>().sizeDelta = new Vector2(200f,areaInfo.Length*85f);
            //}
            for (int i = 0; i < _infoTexts.Length; i++)
            {
                if (i < areaInfo.Length)
                {
                    _infoTexts[i].gameObject.SetActive(true);
                    _infoButtonsParent[i].gameObject.SetActive(true);
                    _infoTexts[i].text = areaInfo[i];

                }
                else
                {
                    _infoTexts[i].gameObject.SetActive(false);
                    _infoButtonsParent[i].gameObject.SetActive(false);

                    _infoTexts[i].text = null;
                    _infoButtonsTexts[i].text = null;
                }
                    
            }
            
        }
    }
    public void MoveObjectInfo(string[] textsForMove,string[] textsForButtons, Enums.PlayerStateType playerStateType)
    {

        CheckShowInfoText = false;
        ShowAreaInfoForTexts(textsForMove);
        ShowAreaInfoForTextsButtons(textsForButtons);
        Player.Instance.MoveObject(gameObject,playerStateType);
        PlaceController.Instance.ActivateDecorationPlane(true);
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
        if (CheckShowInfoText)
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
        
        Debug.Log("nohitçalışıyorbe");

        
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

    public void SetCleanRateText(float rate)
    {
        _cleanRateText.text = "% " + ((int)(rate*100)).ToString();
    }
    
}

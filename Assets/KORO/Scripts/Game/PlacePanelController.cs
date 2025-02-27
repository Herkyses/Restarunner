 using System;
 using System.Collections;
using System.Collections.Generic;
 using System.Numerics;
 using DG.Tweening;
 using TMPro;
 using UnityEngine;
 using UnityEngine.Serialization;
 using Zenject;
 using Vector2 = UnityEngine.Vector2;

 public class PlacePanelController : MonoBehaviour,IPanel
{
    public List<FoodIngredient> FoodIngredients;
    public List<SingleShopItem> OwnerFoodIngredients;
    public List<SingleShopItem> OpeningShopItems;
    public List<SingleShopingCardItem> ShopCardItems;
    //public List<FoodIngredient> FoodIngredients;
    public Transform _panel;
    public Transform _ownerFoodIngredients;
    public Transform TabButtonParent;
    public RectTransform[] TabButtonsRecttransforms;
    [FormerlySerializedAs("_singlePlaceItemParentTransform")] public Transform SingleShopItemParentTransform;
    public SingleShopItem SingleShopItemPf;
    public Transform SingleShopingCardItemParentTransform;
    public SingleShopingCardItem SingleShopingCardItemPf;
    private Vector2 firstSizeDelta;
    private ShopManager _shopManager;
    private Tween _shopPanelMove;
    [SerializeField] private TextMeshProUGUI _totalCostText;
    
    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
    

    private void Start()
    {
        _shopManager = ControllerManager.Instance.ShopManagerPanel;
        var rectTransforms = TabButtonParent.GetComponentsInChildren<SingleTabButton>();
        TabButtonsRecttransforms = new RectTransform[rectTransforms.Length];
        for (int i = 0; i < rectTransforms.Length; i++)
        {
            TabButtonsRecttransforms[i] = rectTransforms[i].GetComponent<RectTransform>();
        }

        firstSizeDelta = TabButtonsRecttransforms[0].sizeDelta;
    }

    private void OnEnable()
    {
        MealManager.UpdateFoodIngredient += UpdateFoodIngredient;
        ShopManager.UpdateShopBasket += UpdateShopingBasket;
    }
    private void OnDisable()
    {
        MealManager.UpdateFoodIngredient -= UpdateFoodIngredient;
        ShopManager.UpdateShopBasket -= UpdateShopingBasket;

    }

    public void UpdateCostText()
    {
        _totalCostText.text = "Total: " +  "<color=green>" + _shopManager._shoppingCardCost.ToString() +"$" +"</color>";
    }
    public void ActivePlacePanel()
    {
        GameSceneCanvas.Instance.CanMove = false;
        GameSceneCanvas.IsCursorVisible?.Invoke(true);
        GameManager.IsAnyPanelOpened?.Invoke(true);
        _panel.gameObject.SetActive(true);
        if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 1)
        {
            InitializeWithButton(1);
        }
    }
    public void DeActivePlacePanel()
    {
        GameSceneCanvas.Instance.CanMove = true;
        GameSceneCanvas.IsCursorVisible?.Invoke(false);
        _panel.gameObject.SetActive(false);
        GameManager.IsAnyPanelOpened?.Invoke(false);
        //OpenPlacePanel();
    }

    public void OpenPlacePanel()
    {
        if (_panel.gameObject.activeSelf)
        {
            //DeActivePlacePanel();
            if (_shopPanelMove != null)
            {
                _shopPanelMove.Kill();
            }
            var panelRect = _panel.GetComponent<RectTransform>();
            _shopPanelMove = panelRect.DOLocalMoveY( - 1200f, 0.3f).OnComplete(DeActivePlacePanel);
        }
        else
        {
            ActivePlacePanel();
            var panelRect = _panel.GetComponent<RectTransform>();
            panelRect.anchoredPosition += Vector2.down*600f; 
            if (_shopPanelMove != null)
            {
                _shopPanelMove.Kill();
            }
            _shopPanelMove = panelRect.DOLocalMoveY(0f, 0.3f);


        }
    }

    
    

    public void Initialize()
    {
        Utilities.DeleteTransformchilds(SingleShopItemParentTransform);
        Utilities.DeleteTransformchilds(SingleShopingCardItemParentTransform);
        InitializeShopPanel(ControllerManager.Instance.ShopManagerPanel.FirstShopItemDatas);
        for (int i = 0; i < FoodIngredients.Count; i++)
        {
            FoodIngredients[i].IngredientValue = 5;
        }
        
    }

    public void UpdateFoodIngredient()
    {
        for (int i = 0; i < OwnerFoodIngredients.Count; i++)
        {
            OwnerFoodIngredients[i].InitializeFoodIngredient(OwnerFoodIngredients[i].OrderType, PlayerPrefsManager.Instance.LoadMeals().meals[i].ingredientQuantity);
        }
    }

    public void InitializeWithButton(int index)
    {
        
        if (index != 1 && PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 1)
        {
            TabButtonsRecttransforms[1].DOSizeDelta(firstSizeDelta * 1.1f, 0.2f);
            return;
        }
        for (int i = 0; i < TabButtonsRecttransforms.Length; i++)
        {
            if (i != index)
            {
                TabButtonsRecttransforms[i].DOSizeDelta(firstSizeDelta, 0.2f);
 
            }
            else
            {
                TabButtonsRecttransforms[i].DOSizeDelta(firstSizeDelta * 1.1f, 0.2f);
 
            }
        }
        
        Utilities.DeleteTransformchilds(SingleShopItemParentTransform);

        switch (index)
        {
            case 0:
                InitializeShopPanel(ControllerManager.Instance.ShopManagerPanel.FirstShopItemDatas);
                break;
            case 1:
                InitializeShopPanel(ControllerManager.Instance.ShopManagerPanel.EnvironmentShopItemDatas);
                break;
            case 2:
                InitializeShopPanel(ControllerManager.Instance.ShopManagerPanel.FoodIngradientShopItemDatas);
                break;
            case 3:
                InitializeShopPanel(ControllerManager.Instance.ShopManagerPanel.PlaceUpgradeDatas);
                break;
            case 4:
                InitializeShopPanel(ControllerManager.Instance.ShopManagerPanel.DecorationDatas);
                break;
        }
    }
    public void InitializeShopPanel(List<ShopItemData> shopItemDatas)
    {
        OpeningShopItems.Clear();
        var placeLevel = PlayerPrefsManager.Instance.LoadPlaceLevel();

        for (int i = 0; i < shopItemDatas.Count; i++)
        {
            if (shopItemDatas[i].PlaceLevel <= placeLevel)
            {
                var singleItem = Instantiate(SingleShopItemPf, SingleShopItemParentTransform);
                singleItem.InitializeSingleShopItem(shopItemDatas[i], Enums.ShopItemUIType.Shop);
                OpeningShopItems.Add(singleItem);
            }
        }
        for (int i = 0; i < shopItemDatas.Count; i++)
        {
            if (shopItemDatas[i].PlaceLevel > placeLevel)
            {
                var singleItem = Instantiate(SingleShopItemPf, SingleShopItemParentTransform);
                singleItem.InitializeSingleShopItem(shopItemDatas[i], Enums.ShopItemUIType.Shop);
                OpeningShopItems.Add(singleItem);
            }
        }
    }

    public void UpdateShopItems()
    {
        for (int i = 0; i < OpeningShopItems.Count; i++)
        {
            OpeningShopItems[i].UpdateShopItem();
        }
    }


    public void BuyButtonPressed()
    {
        ControllerManager.Instance.ShopManagerPanel.BuyShoppingBasketButtonPressed();
    }
    public void UpdateShopingBasket(ShopItemData shopItemData)
    {
        //Utilities.DeleteTransformchilds(SingleShopingCardItemParentTransform);
        //ShopCardItems.Clear();
        var shopManager = ControllerManager.Instance.ShopManagerPanel;
        //shopManager._shoppingCardCost = 0;
        var shopingCard = Instantiate(SingleShopingCardItemPf, SingleShopingCardItemParentTransform);
        ShopCardItems.Add(shopingCard);
        shopingCard.Initliaze(shopItemData);
        //shopManager._shoppingCardCost += shopItemData.ShopItemPrice;
    }

    public void CheckShopBasket(ShopItemData shopItemData)
    {
        //_shopManager._shoppingCardCost = 0;
        for (int i = 0; i < ShopCardItems.Count; i++)
        {
            if (shopItemData == ShopCardItems[i].ShopItem)
            {
                ShopCardItems[i].count++;
                ShopCardItems[i].UpdateCountText();
                //_shopManager._shoppingCardCost += _shopManager.ShoppingBasket[i].ShopItemPrice*(ShopCardItems[i].count+1);
            }
        }
    }
}
[System.Serializable]
 public class FoodIngredient
 {
     public Enums.OrderType OrderType;
     public int IngredientValue;
 }

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<ShopItem> FirstShopItems;
    public List<ShopItemData> FirstShopItemDatas;
    public List<ShopItemData> EnvironmentShopItemDatas;
    public List<ShopItemData> FoodIngradientShopItemDatas;
    public List<ShopItemData> PlaceUpgradeDatas;
    public List<ShopItemData> DecorationDatas;
    public List<ShopItemData> ShoppingBasket;

    public float ShoppingBasketPrice;
    public GameObject OrderBoxObject;
    public Transform ShopOrderTransform;

    public static Action<ShopItemData> UpdateShopBasket;
    public static Action UpgradedRestaurant;
    
    private const int TutorialStepPlaceUpgrade = 2;

    public float _shoppingCardCost;

    
    public void CreateShopItem(ShopItemData shopItemData,SingleShopItem singleShopItem)
    {
        switch (shopItemData.ItemType)
        {
            case Enums.ShopItemType.Chef:
                break;
            case Enums.ShopItemType.Table:
                if (shopItemData.ShopItemPrice <= PlayerPrefsManager.Instance.LoadPlayerMoney())
                {
                    AddShoppingToBasket(shopItemData);
                }
                break;
            case Enums.ShopItemType.Waiter:
                break;
            
            case Enums.ShopItemType.FoodIngredient:
                if (shopItemData.ShopItemPrice <= PlayerPrefsManager.Instance.LoadPlayerMoney())
                {
                    AddShoppingToBasket(shopItemData);
                }
                break;
            case Enums.ShopItemType.PlaceUpgrade:
                var playerPrefs = PlayerPrefsManager.Instance;
                if (shopItemData.ShopItemPrice <= playerPrefs.LoadPlayerMoney()  && playerPrefs.LoadPlaceLevel() == shopItemData.PlaceLevel)
                {
                    HandlePlaceUpgrade(shopItemData,singleShopItem);
                }
                break;
            case Enums.ShopItemType.Decoration:
                if (shopItemData.ShopItemPrice <= PlayerPrefsManager.Instance.LoadPlayerMoney())
                {
                    //CreateOrderBox(shopItemData);
                    AddShoppingToBasket(shopItemData);
                }
                break;
            
        }
        UpdateShopingBasket();
    }

    public void HandlePlaceUpgrade(ShopItemData shopItemData,SingleShopItem singleShopItem)
    {
        singleShopItem.IsButtonActive = false;
        GameManager.PayedOrderBill?.Invoke(-shopItemData.ShopItemPrice);
        var level = PlayerPrefsManager.Instance.LoadPlaceLevel() +1;
        PlayerPrefsManager.Instance.SavePlaceLevel(level);
        UpgradedRestaurant?.Invoke();
        BuyPlaceUpgrade();
        ControllerManager.Instance.PlacePanelController.UpdateShopItems();
        GameDataManager.Instance.UpdateOpenFoodDatas();
    }

    public void AddShoppingToBasket(ShopItemData shopItemData)
    {
        if (!ShoppingBasket.Contains(shopItemData))
        {
            //_shoppingCardCost += shopItemData.ShopItemPrice;
            ShoppingBasket.Add(shopItemData);
            UpdateShopBasket?.Invoke(shopItemData);

        }
        else
        {
            ControllerManager.Instance.PlacePanelController.CheckShopBasket(shopItemData);
        }
    }
    
    public void CreateOrderBox(ShopItemData shopItemData)
    {
        //var item = Instantiate(OrderBoxObject);
        GameManager.PayedOrderBill?.Invoke(-shopItemData.ShopItemPrice);
        var item = PoolManager.Instance.GetFromPoolForOrderBox();
        item.GetComponent<OrderBox>().SetShopItemData(shopItemData);
        item.transform.position = ShopOrderTransform.position;
        MapManager.Instance.SaveMap();
        if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 1)
        {
            PlayerPrefsManager.Instance.SavePlayerPlayerTutorialStep(TutorialStepPlaceUpgrade);
            TutorialManager.Instance.Initiliaze();
        }
        
    }

    public void BuyPlaceUpgrade()
    {
        ControllerManager.Instance.PlaceController.Initialize();
    }

    public void BuyShoppingBasketButtonPressed()
    {
        if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 1)
        {
            if (ControllerManager.Instance.PlacePanelController.ShopCardItems[0].count != 1)
            {
                return;
            }
        }
        var placepanelController = ControllerManager.Instance.PlacePanelController;
        if (_shoppingCardCost <= PlayerPrefsManager.Instance.LoadPlayerMoney())
        {
            _shoppingCardCost = 0;
            
            for (int i = 0; i < placepanelController.ShopCardItems.Count; i++)
            {
                for (int j = 0; j < placepanelController.ShopCardItems[i].count+1; j++)
                {
                    CreateOrderBox(placepanelController.ShopCardItems[i].ShopItem);

                }
            }
            ShoppingBasket.Clear();
        }
        placepanelController.ShopCardItems.Clear();
        Utilities.DeleteTransformchilds(placepanelController.SingleShopingCardItemParentTransform);
        placepanelController.UpdateCostText();
        
    }

    public void UpdateShopingBasket()
    {
        _shoppingCardCost = ControllerManager.Instance.PlacePanelController.ShopCardItems
            .Sum(item => item.ShopItem.ShopItemPrice * (item.count + 1));
        ControllerManager.Instance.PlacePanelController.UpdateCostText();
    }


    
    private void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKey(KeyCode.L))
        {
            Player.Instance.GainMoney(10f);
        }
        #endif
    }
}
[System.Serializable]
public class ShopItem
{
    public Enums.ShopItemType ItemType;
    public float ShopItemPrice;
    public Sprite ShopItemIcon;
    public GameObject ItemObject;
}

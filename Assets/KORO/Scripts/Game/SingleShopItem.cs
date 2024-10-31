using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleShopItem : MonoBehaviour
{
    public Image Icon;
    public Image LockerIcon;
    public float Price;
    public Enums.ShopItemType ItemType;
    public Enums.ShopItemUIType ShopItemUIType;
    [SerializeField] private ShopItemData _shopItemData;
    public Enums.OrderType OrderType;
    public bool IsButtonActive;

    public TextMeshProUGUI PriceText;


    public void InitializeSingleShopItem(ShopItemData shopItem,Enums.ShopItemUIType shopItemUIType = Enums.ShopItemUIType.Inventory)
    {
        
        Price = shopItem.ShopItemPrice;
        ItemType = shopItem.ItemType;
        Icon.sprite = shopItem.ShopItemIcon;
        _shopItemData = shopItem;
        ShopItemUIType = shopItemUIType;
        OrderType = shopItem.ItemOrderType;
        PriceText.text = Price.ToString() + " $";
        CheckShopItem();
    }

    public void CheckShopItem()
    {
        if (ShopItemUIType == Enums.ShopItemUIType.Inventory)
        {
            LockerIcon.gameObject.SetActive(false);
        }
        else
        {
            UpdateShopItem();
        }
    }
    public void UpdateShopItem()
    {
        var placeLevel = PlayerPrefsManager.Instance.LoadPlaceLevel();
        if (_shopItemData.ItemType == Enums.ShopItemType.PlaceUpgrade)
        {
            if (_shopItemData.PlaceLevel < placeLevel)
            {
                IsButtonActive = false;
                LockerIcon.gameObject.SetActive(false);

            }
            else if (_shopItemData.PlaceLevel == placeLevel)
            {
                IsButtonActive = true;
                LockerIcon.gameObject.SetActive(false);
            }
            else
            {
                IsButtonActive = false;

            }
            
        }
        else
        {
            if (_shopItemData.PlaceLevel <= placeLevel)
            {
                IsButtonActive = true;
                LockerIcon.gameObject.SetActive(false);
            }
            else
            {
                IsButtonActive = false;

            }
        }
    }

    public void InitializeFoodIngredient(Enums.OrderType orderType, int foodGredientvalue)
    {
        // Price = shopItem.ShopItemPrice;
        ItemType = Enums.ShopItemType.FoodIngredient;
        Icon.sprite = GameDataManager.Instance.GetFoodSprite(orderType);
        // _shopItemData = shopItem;
        OrderType = orderType;
        PriceText.text = foodGredientvalue.ToString();
    }
    public void SinglePlaceItemPressed()
    {
        if (!IsButtonActive || ShopItemUIType == Enums.ShopItemUIType.Inventory)
        {
            return;
        }
        ControllerManager.Instance.ShopManagerPanel.CreateShopItem(_shopItemData,this);
    }
    
}

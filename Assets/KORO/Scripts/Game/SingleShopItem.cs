using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleShopItem : MonoBehaviour
{
    public Image Icon;
    public float Price;
    public Enums.ShopItemType ItemType;
    [SerializeField] private ShopItemData _shopItemData;
    public Enums.OrderType OrderType;
    public bool IsButtonActive;

    public TextMeshProUGUI PriceText;


    public void InitializeSingleShopItem(ShopItemData shopItem)
    {
        IsButtonActive = true;
        Price = shopItem.ShopItemPrice;
        ItemType = shopItem.ItemType;
        Icon.sprite = shopItem.ShopItemIcon;
        _shopItemData = shopItem;
        OrderType = shopItem.ItemOrderType;
        PriceText.text = Price.ToString();
        if (shopItem.ItemType == Enums.ShopItemType.PlaceUpgrade)
        {
            if (shopItem.PlaceLevel <= PlayerPrefsManager.Instance.LoadPlaceLevel())
            {
                IsButtonActive = false;
            }
        }
    }

    public void InitializeFoodIngredient(Enums.OrderType orderType, int foodGredientvalue)
    {
        // Price = shopItem.ShopItemPrice;
        ItemType = Enums.ShopItemType.FoodIngredient;
        Icon.sprite = GetFoodSprite(orderType);
        // _shopItemData = shopItem;
        OrderType = orderType;
        PriceText.text = foodGredientvalue.ToString();
    }
    public void SinglePlaceItemPressed()
    {
        if (!IsButtonActive)
        {
            return;
        }
        ShopManager.Instance.CreateShopItem(_shopItemData,this);
    }
    public Sprite GetFoodSprite(Enums.OrderType orderType)
    {
        
        switch (orderType)
        {
            case Enums.OrderType.Burger:
                return ShopManager.Instance.FoodIngradientShopItemDatas[0].ShopItemIcon;
            case Enums.OrderType.Pizza:
                return ShopManager.Instance.FoodIngradientShopItemDatas[1].ShopItemIcon;
            case Enums.OrderType.Taco:
                return ShopManager.Instance.FoodIngradientShopItemDatas[2].ShopItemIcon;
            case Enums.OrderType.Croissant:
                return ShopManager.Instance.FoodIngradientShopItemDatas[3].ShopItemIcon;
            case Enums.OrderType.HotDog:
                return ShopManager.Instance.FoodIngradientShopItemDatas[4].ShopItemIcon;
            case Enums.OrderType.Sandwich:
                return ShopManager.Instance.FoodIngradientShopItemDatas[5].ShopItemIcon;
            case Enums.OrderType.Chicken:
                return ShopManager.Instance.FoodIngradientShopItemDatas[6].ShopItemIcon;
        }

        return null;
    }
}

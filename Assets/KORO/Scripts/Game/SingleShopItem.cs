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

    public TextMeshProUGUI PriceText;


    public void InitializeSingleShopItem(ShopItemData shopItem)
    {
        Price = shopItem.ShopItemPrice;
        ItemType = shopItem.ItemType;
        Icon.sprite = shopItem.ShopItemIcon;
        _shopItemData = shopItem;
        OrderType = shopItem.ItemOrderType;
        PriceText.text = Price.ToString();
    }
    public void SinglePlaceItemPressed()
    {
        ShopManager.Instance.CreateShopItem(_shopItemData);
    }
}

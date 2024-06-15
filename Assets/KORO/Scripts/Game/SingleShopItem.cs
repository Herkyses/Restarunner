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


    public TextMeshProUGUI PriceText;


    public void InitializeSingleShopItem(ShopItemData shopItem)
    {
        Price = shopItem.ShopItemPrice;
        ItemType = shopItem.ItemType;
        Icon.sprite = shopItem.ShopItemIcon;
        _shopItemData = shopItem;
    }
    public void SinglePlaceItemPressed()
    {
        ShopManager.Instance.CreateShopItem(_shopItemData);
    }
}

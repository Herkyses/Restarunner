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


    public TextMeshProUGUI PriceText;


    public void InitializeSingleShopItem(ShopItem shopItem)
    {
        Price = shopItem.ShopItemPrice;
        ItemType = shopItem.ItemType;
    }
    public void SinglePlaceItemPressed()
    {
        
    }
}

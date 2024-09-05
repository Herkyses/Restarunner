using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleShopingCardItem : MonoBehaviour
{

    public Image ShopingCarItemIcon;
    public float ShopingCarItemPrice;
    public ShopItemData ShopItem;

    public void RemoveButtonPressed()
    {
        ShopManager.Instance.ShoppingBasket.Remove(ShopItem);
        ShopManager.UpdateShopBasket?.Invoke();
    }
}

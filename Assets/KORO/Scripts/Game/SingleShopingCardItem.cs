using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleShopingCardItem : MonoBehaviour
{

    public Image ShopingCarItemIcon;
    public float ShopingCarItemPrice;
    public ShopItemData ShopItem;
    public int count;

    public void RemoveButtonPressed()
    {
        if (count + 1 > 1)
        {
            count--;
            ShopManager.Instance.UpdateShopingBasket();

        }
        else
        {
            ShopManager.Instance.ShoppingBasket.Remove(ShopItem);
            PlacePanelController.Instance.ShopCardItems.Remove(this);
            ShopManager.Instance.UpdateShopingBasket();

            Destroy(gameObject);
        }

        //ShopManager.UpdateShopBasket?.Invoke();
    }

    public void Initliaze(ShopItemData shopItemData)
    {
        ShopItem = shopItemData;
        ShopingCarItemIcon.sprite = shopItemData.ShopItemIcon;
        ShopingCarItemPrice = shopItemData.ShopItemPrice;
        
    }
}

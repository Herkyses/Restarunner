using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleShopingCardItem : MonoBehaviour
{

    public Image ShopingCarItemIcon;
    public float ShopingCarItemPrice;
    public ShopItemData ShopItem;
    public TextMeshProUGUI ShopItemCountText;
    public TextMeshProUGUI TotalCost;
    public int count;

    public void RemoveButtonPressed()
    {
        if (count + 1 > 1)
        {
            count--;
            ControllerManager.Instance.ShopManagerPanel.UpdateShopingBasket();
            UpdateCountText();

        }
        else
        {
            ControllerManager.Instance.ShopManagerPanel.ShoppingBasket.Remove(ShopItem);
            ControllerManager.Instance.PlacePanelController.ShopCardItems.Remove(this);
            ControllerManager.Instance.ShopManagerPanel.UpdateShopingBasket();

            Destroy(gameObject);
        }

        //ShopManager.UpdateShopBasket?.Invoke();
    }

    public void PlusButtonPressed()
    {
       
        count++;
        UpdateCountText();
        //ShopManager.Instance.ShoppingBasket.Add(ShopItem);
        ControllerManager.Instance.ShopManagerPanel.UpdateShopingBasket();

    }

    public void Initliaze(ShopItemData shopItemData)
    {
        ShopItem = shopItemData;
        ShopingCarItemIcon.sprite = shopItemData.ShopItemIcon;
        ShopingCarItemPrice = shopItemData.ShopItemPrice;
        UpdateCountText();
    }

    public void UpdateCountText()
    {
        ShopItemCountText.text = (count+1).ToString();
        TotalCost.text = ((count + 1) * ShopingCarItemPrice).ToString()+ "$";
    }
}

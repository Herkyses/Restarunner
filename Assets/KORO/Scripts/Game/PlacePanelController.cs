 using System;
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.Serialization;

 public class PlacePanelController : MonoBehaviour
{
    public static PlacePanelController Instance;
    public List<FoodIngredient> FoodIngredients;
    //public List<FoodIngredient> FoodIngredients;
    public Transform _panel;
    [FormerlySerializedAs("_singlePlaceItemParentTransform")] public Transform SingleShopItemParentTransform;
    public SingleShopItem SingleShopItemPf;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void ActivePlacePanel()
    {
        GameSceneCanvas.Instance.CanMove = false;
        _panel.gameObject.SetActive(true);
    }
    public void DeActivePlacePanel()
    {
        GameSceneCanvas.Instance.CanMove = true;
        _panel.gameObject.SetActive(false);

    }

    public void Initialize()
    {
        DeleteChilds();
        InitializeShopPanel(ShopManager.Instance.FirstShopItemDatas);
        for (int i = 0; i < FoodIngredients.Count; i++)
        {
            FoodIngredients[i].IngredientValue = 5;
        }
    }

    public void InitializeWithButton(int index)
    {
        DeleteChilds();
        switch (index)
        {
            case 0:
                InitializeShopPanel(ShopManager.Instance.FirstShopItemDatas);
                break;
            case 1:
                InitializeShopPanel(ShopManager.Instance.EnvironmentShopItemDatas);
                break;
        }
    }
    public void InitializeShopPanel(List<ShopItemData> shopItemDatas)
    {
        for (int i = 0; i < shopItemDatas.Count; i++)
        {
            var singleItem = Instantiate(SingleShopItemPf, SingleShopItemParentTransform);
            singleItem.InitializeSingleShopItem(shopItemDatas[i]);
        }
    }
    public void DecreeseIngredient(Enums.OrderType orderType)
    {
        for (int i = 0; i < FoodIngredients.Count; i++)
        {
            if (orderType == FoodIngredients[i].OrderType)
            {
                FoodIngredients[i].IngredientValue--;
            }
        }
    }
    public void DeleteChilds()
    {
        var orderArray = SingleShopItemParentTransform.GetComponentsInChildren<SingleShopItem>();
        if (orderArray.Length > 0)
        {
            for (int i = 0; i < orderArray.Length; i++)
            {
                Destroy(orderArray[i].gameObject);
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

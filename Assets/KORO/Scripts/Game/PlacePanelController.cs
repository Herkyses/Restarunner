 using System;
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.Serialization;

 public class PlacePanelController : MonoBehaviour
{
    public static PlacePanelController Instance;
    public List<FoodIngredient> FoodIngredients;
    public List<SingleShopItem> OwnerFoodIngredients;
    public List<SingleShopItem> OpeningShopItems;
    //public List<FoodIngredient> FoodIngredients;
    public Transform _panel;
    public Transform _ownerFoodIngredients;
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

    private void OnEnable()
    {
        MealManager.UpdateFoodIngredient += UpdateFoodIngredient;
    }
    private void OnDisable()
    {
        MealManager.UpdateFoodIngredient -= UpdateFoodIngredient;
    }

    public void ActivePlacePanel()
    {
        GameSceneCanvas.Instance.CanMove = false;
        _panel.gameObject.SetActive(true);
        if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 1)
        {
            InitializeWithButton(1);
        }
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

        for (int i = 0; i < PlayerPrefsManager.Instance.LoadMeals().meals.Count; i++)
        {
            var singleOwnerfoodGredient = Instantiate(SingleShopItemPf, _ownerFoodIngredients);
            singleOwnerfoodGredient.transform.SetParent(_ownerFoodIngredients);
            singleOwnerfoodGredient.InitializeFoodIngredient(PlayerPrefsManager.Instance.LoadMeals().meals[i].mealName,PlayerPrefsManager.Instance.LoadMeals().meals[i].ingredientQuantity);
            OwnerFoodIngredients.Add(singleOwnerfoodGredient);

        }
    }

    public void UpdateFoodIngredient()
    {
        for (int i = 0; i < OwnerFoodIngredients.Count; i++)
        {
            OwnerFoodIngredients[i].InitializeFoodIngredient(OwnerFoodIngredients[i].OrderType, PlayerPrefsManager.Instance.LoadMeals().meals[i].ingredientQuantity);
        }
    }

    public void InitializeWithButton(int index)
    {
        if (index != 1 && PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 1)
        {
            return;
        }
        DeleteChilds();
        switch (index)
        {
            case 0:
                InitializeShopPanel(ShopManager.Instance.FirstShopItemDatas);
                break;
            case 1:
                InitializeShopPanel(ShopManager.Instance.EnvironmentShopItemDatas);
                break;
            case 2:
                InitializeShopPanel(ShopManager.Instance.FoodIngradientShopItemDatas);
                break;
            case 3:
                InitializeShopPanel(ShopManager.Instance.PlaceUpgradeDatas);
                break;
            case 4:
                InitializeShopPanel(ShopManager.Instance.DecorationDatas);
                break;
        }
    }
    public void InitializeShopPanel(List<ShopItemData> shopItemDatas)
    {
        OpeningShopItems.Clear();
        for (int i = 0; i < shopItemDatas.Count; i++)
        {
            var singleItem = Instantiate(SingleShopItemPf, SingleShopItemParentTransform);
            singleItem.InitializeSingleShopItem(shopItemDatas[i]);
            OpeningShopItems.Add(singleItem);
        }
    }

    public void UpdateShopItems()
    {
        for (int i = 0; i < OpeningShopItems.Count; i++)
        {
            OpeningShopItems[i].UpdateShopItem();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public List<ShopItem> FirstShopItems;
    public List<ShopItemData> FirstShopItemDatas;
    public List<ShopItemData> EnvironmentShopItemDatas;
    public List<ShopItemData> FoodIngradientShopItemDatas;
    public List<ShopItemData> PlaceUpgradeDatas;
    public Transform ShopOrderTransform;
    // Start is called before the first frame update
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

    public void CreateShopItem(ShopItemData shopItemData)
    {
        switch (shopItemData.ItemType)
        {
            case Enums.ShopItemType.Chef:
                break;
            case Enums.ShopItemType.Table:
                if (shopItemData.ShopItemPrice <= PlayerPrefsManager.Instance.LoadPlayerMoney())
                {
                    GameManager.PayedOrderBill?.Invoke(-shopItemData.ShopItemPrice);
                    CreateTable(shopItemData);
                }
                break;
            case Enums.ShopItemType.Waiter:
                break;
            
            case Enums.ShopItemType.FoodIngredient:
                if (shopItemData.ShopItemPrice <= PlayerPrefsManager.Instance.LoadPlayerMoney())
                {
                    GameManager.PayedOrderBill?.Invoke(-shopItemData.ShopItemPrice);
                    BuyFoodIngredient(shopItemData);
                }
                break;
            case Enums.ShopItemType.PlaceUpgrade:
                break;
            
        }
    }

    public void BuyFoodIngredient(ShopItemData shopItemData)
    {
        MealManager mealManager = new MealManager();

        // Yemekleri yükle ve Burger yap
        mealManager.LoadMeals();

        mealManager.MakeMeal(shopItemData.ItemOrderType,1);

        // Veriyi tekrar yükle ve kalan miktarları kontrol et
        MealsList loadedMealsList = PlayerPrefsManager.Instance.LoadMeals();
        if (loadedMealsList != null)
        {
            foreach (Meal meal in loadedMealsList.meals)
            {
                Debug.Log("Meal: " + meal.mealName + ", Ingredient Quantity: " + meal.ingredientQuantity);
            }
        }
        else
        {
            Debug.Log("No data found.");
        }
    }

    public void CreateTable(ShopItemData shopItemData)
    {
        var item = Instantiate(shopItemData.ItemObject);
        item.transform.position = ShopOrderTransform.position;
        
    }

    
}
[System.Serializable]
public class ShopItem
{
    public Enums.ShopItemType ItemType;
    public float ShopItemPrice;
    public Sprite ShopItemIcon;
    public GameObject ItemObject;
}

using System;
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
    public List<ShopItemData> DecorationDatas;
    public List<ShopItemData> ShoppingBasket;

    public float ShoppingBasketPrice;
    public GameObject OrderBoxObject;
    public Transform ShopOrderTransform;

    public static Action UpdateShopBasket;

    public float _shoppingCardCost;
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
    
    public void CreateShopItem(ShopItemData shopItemData,SingleShopItem singleShopItem)
    {
        switch (shopItemData.ItemType)
        {
            case Enums.ShopItemType.Chef:
                break;
            case Enums.ShopItemType.Table:
                if (shopItemData.ShopItemPrice <= PlayerPrefsManager.Instance.LoadPlayerMoney())
                {
                    //GameManager.PayedOrderBill?.Invoke(-shopItemData.ShopItemPrice);
                    //CreateOrderBox(shopItemData);
                    if (!ShoppingBasket.Contains(shopItemData))
                    {
                        _shoppingCardCost += shopItemData.ShopItemPrice;
                        ShoppingBasket.Add(shopItemData);
                        UpdateShopBasket?.Invoke();

                    }

                }
                break;
            case Enums.ShopItemType.Waiter:
                break;
            
            case Enums.ShopItemType.FoodIngredient:
                if (shopItemData.ShopItemPrice <= PlayerPrefsManager.Instance.LoadPlayerMoney())
                {
                    //GameManager.PayedOrderBill?.Invoke(-shopItemData.ShopItemPrice);
                    //BuyFoodIngredient(shopItemData);
                    if (!ShoppingBasket.Contains(shopItemData))
                    {
                        _shoppingCardCost += shopItemData.ShopItemPrice;
                        ShoppingBasket.Add(shopItemData);
                        UpdateShopBasket?.Invoke();

                    }

                }
                break;
            case Enums.ShopItemType.PlaceUpgrade:
                var playerPrefs = PlayerPrefsManager.Instance;
                if (shopItemData.ShopItemPrice <= playerPrefs.LoadPlayerMoney() && playerPrefs.LoadPlaceLevel() < playerPrefs.LoadPlaceRubbishLevel() && playerPrefs.LoadPlaceLevel() == shopItemData.PlaceLevel)
                {
                    singleShopItem.IsButtonActive = false;
                    GameManager.PayedOrderBill?.Invoke(-shopItemData.ShopItemPrice);
                    var level = PlayerPrefsManager.Instance.LoadPlaceLevel() +1;
                    PlayerPrefsManager.Instance.SavePlaceLevel(level);

                    BuyPlaceUpgrade();
                    PlacePanelController.Instance.UpdateShopItems();
                    GameDataManager.Instance.UpdateOpenFoodDatas();
                }
                break;
            case Enums.ShopItemType.Decoration:
                if (shopItemData.ShopItemPrice <= PlayerPrefsManager.Instance.LoadPlayerMoney())
                {
                    //CreateOrderBox(shopItemData);
                    if (!ShoppingBasket.Contains(shopItemData))
                    {
                        _shoppingCardCost += shopItemData.ShopItemPrice;
                        ShoppingBasket.Add(shopItemData);
                        UpdateShopBasket?.Invoke();

                    }

                }
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
    public void CreateOrderBox(ShopItemData shopItemData)
    {
        //var item = Instantiate(OrderBoxObject);
        GameManager.PayedOrderBill?.Invoke(-shopItemData.ShopItemPrice);
        var item = PoolManager.Instance.GetFromPoolForOrderBox();
        item.GetComponent<OrderBox>().SetShopItemData(shopItemData);
        item.transform.position = ShopOrderTransform.position;
        MapManager.Instance.SaveMap();
        if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 1)
        {
            PlayerPrefsManager.Instance.SavePlayerPlayerTutorialStep(2);
            TutorialManager.Instance.Initiliaze();
        }
        
    }

    public void BuyPlaceUpgrade()
    {
        PlaceController.Instance.Initialize();
    }

    public void BuyShoppingBasketButtonPressed()
    {
        if (_shoppingCardCost <= PlayerPrefsManager.Instance.LoadPlayerMoney())
        {
            _shoppingCardCost = 0;
            for (int i = 0; i < ShoppingBasket.Count; i++)
            {
                CreateOrderBox(ShoppingBasket[i]);
            }
            ShoppingBasket.Clear();
        }
        
    }


    
    private void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            Player.Instance.GainMoney(10f);
        }
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

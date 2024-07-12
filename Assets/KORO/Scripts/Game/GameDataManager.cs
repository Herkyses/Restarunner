using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDataManager : MonoBehaviour
{
    
    
    public List<OrderData> FoodDatas;
    public FoodTable FoodTablePf;

    public static GameDataManager Instance;
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

    public Food GetFood(Enums.OrderType orderType)
    {
        switch (orderType)
        {
            case Enums.OrderType.Pizza:
                return FoodDatas[0].Food;
            case Enums.OrderType.Burger:
                return FoodDatas[1].Food;
            case Enums.OrderType.Chicken:
                return FoodDatas[2].Food;
            case Enums.OrderType.Croissant:
                return FoodDatas[3].Food;
            case Enums.OrderType.HotDog:
                return FoodDatas[4].Food;
            case Enums.OrderType.Sandwich:
                return FoodDatas[5].Food;
            case Enums.OrderType.Taco:
                return FoodDatas[6].Food;
            
        }

        return null;
    }

    public float GetOrderBill(Enums.OrderType orderType)
    {
        for (int i = 0; i < FoodDatas.Count; i++)
        {
            if (FoodDatas[i].OrderType == orderType)
            {
                return FoodDatas[i].OrderPrice;
            }
        }

        return -1;
    }

    public Sprite GetFoodSprite(Enums.OrderType orderType)
    {
        
        switch (orderType)
        {
            case Enums.OrderType.Pizza:
                return FoodDatas[0].FoodIcon;
                break;
            case Enums.OrderType.Burger:
                return FoodDatas[1].FoodIcon;
                break;
            case Enums.OrderType.Chicken:
                return FoodDatas[2].FoodIcon;
                break;
            case Enums.OrderType.Croissant:
                return FoodDatas[3].FoodIcon;
                break;
            case Enums.OrderType.HotDog:
                return FoodDatas[4].FoodIcon;
                break;
            case Enums.OrderType.Sandwich:
                return FoodDatas[5].FoodIcon;
                break;
            case Enums.OrderType.Taco:
                return FoodDatas[6].FoodIcon;
                break;
            
        }

        return null;
    }
    public float GetFoodPrice(Enums.OrderType orderType)
    {
        
        switch (orderType)
        {
            case Enums.OrderType.Pizza:
                return FoodDatas[0].OrderPrice;
                break;
            case Enums.OrderType.Burger:
                return FoodDatas[1].OrderPrice;
                break;
            case Enums.OrderType.Chicken:
                return FoodDatas[2].OrderPrice;
                break;
            case Enums.OrderType.Croissant:
                return FoodDatas[3].OrderPrice;
                break;
            case Enums.OrderType.HotDog:
                return FoodDatas[4].OrderPrice;
                break;
            case Enums.OrderType.Sandwich:
                return FoodDatas[5].OrderPrice;
                break;
            case Enums.OrderType.Taco:
                return FoodDatas[6].OrderPrice;
                break;
            
        }

        return 0;
    }
}

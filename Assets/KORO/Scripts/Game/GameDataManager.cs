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
                break;
            case Enums.OrderType.Burger:
                return FoodDatas[1].Food;
                break;
            
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
                return FoodDatas[1].FoodIcon;
                break;
            case Enums.OrderType.Croissant:
                return FoodDatas[1].FoodIcon;
                break;
            case Enums.OrderType.Sandwich:
                return FoodDatas[1].FoodIcon;
                break;
            case Enums.OrderType.Taco:
                return FoodDatas[1].FoodIcon;
                break;
            case Enums.OrderType.HotDog:
                return FoodDatas[1].FoodIcon;
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
            
        }

        return 0;
    }
}

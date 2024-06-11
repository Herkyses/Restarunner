using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDataManager : MonoBehaviour
{
    public List<OrderDataStruct> Orders;
    public List<Food> Foods;
    public List<Sprite> FoodsImages;
    public List<OrderData> FoodDatas;

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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        if (orderType == Enums.OrderType.Burger)
        {
            return FoodsImages[1];
        }
        else if (orderType == Enums.OrderType.Pizza)
        {
            return FoodsImages[0];
        }
        else
        {
            return null;
        } 
    }
}

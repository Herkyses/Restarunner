using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDataManager : MonoBehaviour
{
    
    
    public List<OrderData> FoodDatas;
    public List<OrderData> OpenFoodDatas = new List<OrderData>();
    public List<SingleIngredient> FoodIngredients = new List<SingleIngredient>();
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

    public void Start()
    {
        UpdateOpenFoodDatas();
    }

    public List<OrderData> UpdateOpenFoodDatas()
    {
        OpenFoodDatas.Clear();

        for (int i = 0; i < FoodDatas.Count; i++)
        {
            if (FoodDatas[i].FoodLevel <= PlayerPrefsManager.Instance.LoadPlaceLevel())
            {
                OpenFoodDatas.Add(FoodDatas[i]);
            }
        }

        return OpenFoodDatas;
    }

    public Food GetFood(Enums.OrderType orderType)
    {
        return FoodDatas[GetFoodDataIndex(orderType)].Food;
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

    public OrderData GetFoodData(Enums.OrderType orderType)
    {
        for (int i = 0; i < FoodDatas.Count; i++)
        {
            if (FoodDatas[i].OrderType == orderType)
            {
                return FoodDatas[i];
            }
        }

        return null;
    }

    public Sprite GetFoodSprite(Enums.OrderType orderType)
    {
        return FoodDatas[GetFoodDataIndex(orderType)].FoodIcon;
    }
    public float GetFoodPrice(Enums.OrderType orderType)
    {
        return FoodDatas[GetFoodDataIndex(orderType)].OrderPrice;
        
    }

    public int GetFoodDataIndex(Enums.OrderType orderType)
    {
        for (int i = 0; i < FoodDatas.Count; i++)
        {
            if (orderType == FoodDatas[i].OrderType)
            {
                return i;
            }
        }
        return -1;
    }

    public MeshFilter GetFoodIngredientMeshFilter(Enums.FoodIngredientType foodIngredientType)
    {
        for (int i = 0; i < FoodIngredients.Count; i++)
        {
            if (foodIngredientType == FoodIngredients[i].IngredientType)
            {
                return FoodIngredients[i].GetComponent<MeshFilter>();
            }
        }

        return null;
    }
}

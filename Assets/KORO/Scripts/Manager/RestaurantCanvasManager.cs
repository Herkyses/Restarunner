using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantCanvasManager : MonoBehaviour
{

    public static RestaurantCanvasManager Instance;
    
    [SerializeField] private Transform _foodsParent;
    [SerializeField] private UISingleFood _uiSingleFoodPf;


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

    public void Initiliaze()
    {
        ClearChilds();
        CreateSingleFoods();
    }

    private void ClearChilds()
    {
        Utilities.DeleteTransformchilds(_foodsParent);
    }

    private void CreateSingleFoods()
    {
        var foodDatas = GameDataManager.Instance.FoodDatas;

        for (int i = 0; i < foodDatas.Count; i++)
        {
            Instantiate(_uiSingleFoodPf, _foodsParent);
        }
    }
}

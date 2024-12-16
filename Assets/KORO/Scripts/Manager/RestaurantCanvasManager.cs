using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantCanvasManager : MonoBehaviour
{

    public static RestaurantCanvasManager Instance;
    
    [SerializeField] private Transform _foodsParent;
    [SerializeField] private UISingleFood _uiSingleFoodPf;
    [SerializeField] private List<UISingleFood> _uiSingleFoodList;
    public GameObject referanceObject;


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
        _uiSingleFoodList.Clear();
        Utilities.DeleteTransformchilds(_foodsParent);
    }

    private void CreateSingleFoods()
    {
        var foodDatas = GameDataManager.Instance.FoodDatas;

        for (int i = 0; i < foodDatas.Count; i++)
        {
            var uiSingle = Instantiate(_uiSingleFoodPf, _foodsParent);
            uiSingle.Initiliaze(foodDatas[i]);
            _uiSingleFoodList.Add(uiSingle);
        }
    }
}

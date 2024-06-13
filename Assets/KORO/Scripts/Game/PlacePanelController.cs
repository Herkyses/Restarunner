 using System;
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePanelController : MonoBehaviour
{
    public static PlacePanelController Instance;
    public List<FoodIngredient> FoodIngredients;
    public Transform _panel;
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
        for (int i = 0; i < FoodIngredients.Count; i++)
        {
            FoodIngredients[i].IngredientValue = 5;
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
}
[System.Serializable]
 public class FoodIngredient
 {
     public Enums.OrderType OrderType;
     public int IngredientValue;
 }

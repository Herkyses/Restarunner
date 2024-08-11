using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDatas : MonoBehaviour
{
    
}
[System.Serializable]
public class MapObject
{
    public string type;
    public float posX;
    public float posY;
    public float posZ;
    public float rotX;
    public float rotY;
    public float rotZ;
    public int tableID;
    public int decorationID;
    public ShopItemData shopItemData;
} 
[System.Serializable]
public class Meal
{
    public Enums.OrderType mealName;
    public int ingredientQuantity;

    public Meal(Enums.OrderType mealName, int ingredientQuantity)
    {
        this.mealName = mealName;
        this.ingredientQuantity = ingredientQuantity;
    }
}

[System.Serializable]
public class MealsList
{
    public List<Meal> meals;

    public MealsList(List<Meal> meals)
    {
        this.meals = meals;
    }
}

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
public class MealIngredient
{
    public Enums.FoodIngredientType mealName;
    public int ingredientQuantity;

    public MealIngredient(Enums.FoodIngredientType mealName, int ingredientQuantity)
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
[System.Serializable]
public class MealIngredientsList
{
    public List<MealIngredient> meals;

    public MealIngredientsList(List<MealIngredient> meals)
    {
        this.meals = meals;
    }
}
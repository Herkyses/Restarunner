using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleGredientShelves : MonoBehaviour,IInterectableObject
{
    public Enums.FoodIngredientType shelveIngredientType;

    public void InterectableObjectRun()
    {
        MealManager.Instance.MakeSingleMealIngredient(shelveIngredientType,1);
    }

    public void ShowOutline(bool active)
    {
        
    }

    public Outline GetOutlineComponent()
    {
        return null;

    }
    public string GetInterectableText()
    {
        return null;

    }
    public string[] GetInterectableTexts()
    {
        return null;

    }
    public string[] GetInterectableButtons()
    {
        return null;
    }
    public void Move()
    {
    
    }
    public void Open()
    {
    
    }
    public Enums.PlayerStateType GetStateType()
    {
        return Enums.PlayerStateType.Free;
    }
}

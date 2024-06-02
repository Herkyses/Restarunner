using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour,IInterectableObject
{
    public Enums.OrderType OrderType;

    public void InterectableObjectRun()
    {
        PlayerOrderController.Instance.OrderFoodList.Add(GetComponent<Food>());
        transform.position = PlayerOrderController.Instance.FoodTransform.position;
        transform.SetParent(PlayerOrderController.Instance.FoodTransform);
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
        return "Take Food";
    }
    
}

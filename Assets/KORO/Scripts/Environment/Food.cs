using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour,IInterectableObject
{
    public Enums.OrderType OrderType;
    public GameObject FoodObject;

    public void InterectableObjectRun()
    {
        PlayerOrderController.Instance.TakeFood(GetComponent<Food>());
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
    public void Move()
    {
        
    }
}

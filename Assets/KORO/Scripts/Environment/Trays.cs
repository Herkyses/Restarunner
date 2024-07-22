using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trays : MonoBehaviour,IInterectableObject
{
    public void InterectableObjectRun()
    {
        if (PlayerOrderController.Instance.FoodTable.gameObject)
        {
            PoolManager.Instance.ReturnToPool(PlayerOrderController.Instance.FoodTable.gameObject);
        }
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
        return Enums.PlayerStateType.Trays;
    }
}

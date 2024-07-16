using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefOrderTable : MonoBehaviour,IInterectableObject
{
    public List<Transform> FoodTransformList = new List<Transform>();
    
    public void InterectableObjectRun()
    {
        GiveChefOrderPanelController.Instance.Panel.gameObject.SetActive(true);
        GameSceneCanvas.Instance.CanMove = false;

        GiveChefOrderPanelController.Instance.OrderList = PlayerOrderController.Instance.OrderList;
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
        return "GiveOrder";
    }

    public void SetOrderToChef(int orderIndex)
    {
        if (GiveChefOrderPanelController.Instance.Orders.OrderDataStructs.Count == 0)
        {
            GiveChefOrderPanelController.Instance.Orders = PlayerOrderController.Instance.OrderList[orderIndex];
        }
    }

    public void Move()
    {
        
    }
    public void Open()
    {
        
    }
}

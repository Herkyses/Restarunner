using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefOrderTable : MonoBehaviour,IInterectableObject
{
    [SerializeField] private string[] texts = new [] {"Give Order "};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    public List<Transform> FoodTransformList = new List<Transform>();
    
    public void InterectableObjectRun()
    {
        GiveChefOrderPanelController.Instance.Panel.gameObject.SetActive(true);
        GameSceneCanvas.Instance.CanMove = false;

        GiveChefOrderPanelController.Instance.OrderList = PlayerOrderController.Instance.OrderList;
    }

    private void Start()
    {
        texts = new []{"Give Order "};
        textsButtons = new []{"E"};
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
    public string[] GetInterectableTexts()
    {
        return texts;
    }
    public string[] GetInterectableButtons()
    {
        return textsButtons;
    }
}

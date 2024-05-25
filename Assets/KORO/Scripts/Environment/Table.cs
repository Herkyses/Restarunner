using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour,IInterectableObject
{
    [SerializeField] private string checkOrder = "Check Order";
    [SerializeField] private List<OrderDataStruct> _orderList ;
    public int TableNumber ;
   
    private void OnEnable()
    {
        Chair.GivedOrder += CreateOrdersWithAction;
    }

    private void OnDisable()
    {
        Chair.GivedOrder -= CreateOrdersWithAction;

    }
    public void CreateOrdersWithAction(int tableNumber)
    {
        if (OrderPanelController.Instance.OpenedTableNumber == tableNumber && tableNumber == TableNumber)
        {
            InterectableObjectRun();
        }
    }

    public void SetOrder(OrderDataStruct singleOrder)
    {
        _orderList.Add(singleOrder);
    }
    public void InterectableObjectRun()
    {
        OrderPanelController.Instance.ShowOrder(_orderList,TableNumber);
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
        return checkOrder;
    }
}

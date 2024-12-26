using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderHandler : MonoBehaviour
{
    [SerializeField] private List<OrderDataStruct> _orderList ;
    [SerializeField] private Table _table;

    public void RemoveOrder(OrderDataStruct orderDataStruct)
    {
        _orderList.Remove(orderDataStruct);
    }
    public void SetAIOrder(AIAreaController aiArea)
    {
        var orderIndex = Random.Range(0, GameDataManager.Instance.OpenFoodDatas.Count);
        var orderType = GameDataManager.Instance.OpenFoodDatas[orderIndex].OrderType;
        
        SetOrderTable(aiArea.AIController, orderType);
    }
    public void SetOrderTable(AIController aiController,Enums.OrderType orderType)
    {
        
        var newOrder = new OrderDataStruct()
        {
            OrderType = orderType,
        };
        aiController.FoodDataStruct = newOrder;
        SetOrder(newOrder);
    }
    public List<OrderDataStruct> GetOrders()
    {
        return _orderList;
    }
    public void SetOrder(OrderDataStruct singleOrder)
    {
        _orderList.Add(singleOrder);
    }
    public void NotifyOrderUpdates()
    {
        TableController.GivedOrderForAIWaiter?.Invoke(_table);

        if (ControllerManager.Instance._orderPanelController.OpenedTableNumber == _table.TableNumber)
        {
            Chair.GivedOrder?.Invoke(_table.TableNumber);
        }
    }
}

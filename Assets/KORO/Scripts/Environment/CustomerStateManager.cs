using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerStateManager : MonoBehaviour
{
    private Table _table;
    public List<Chair> ChairList;
    public List<AIController> _aiControllerList ;
    [SerializeField] private List<OrderDataStruct> _orderList ;


    public void StartState(AIAreaController aiArea, Table table)
    {
        _table = table;  ///////geçiciiiiii
        AssignAIToChairAndOrder(aiArea);
        NotifyOrderUpdates();
    }
    public bool CheckAllCustomerFinishedFood() =>
        _aiControllerList.Count > 0 && _aiControllerList.TrueForAll(ai => ai.IsFinishedFood);
    
    private void AssignAIToChairAndOrder(AIAreaController aiArea)
    {
        var chair = CheckChairAvailable();
        if (chair == null) return;

        AssignAIToChair(aiArea, chair);
        SetAIOrder(aiArea);
    }
    public Chair CheckChairAvailable()
    {
        for (int i = 0; i < ChairList.Count; i++)
        {
            if (ChairList[i].isChairAvailable)
            {
                return ChairList[i];
            }
        }

        return null;
    }
    public List<OrderDataStruct> GetOrders()
    {
        return _orderList;
    }
    private void AssignAIToChair(AIAreaController aiArea, Chair chair)
    {
        chair.isChairAvailable = false;
        aiArea.AIController.AssignToChair(aiArea,chair.transform);
        _aiControllerList.Add(aiArea.GetComponent<AIController>());
        aiArea.AIController.SetTableInfo(_table, chair);
    }
    private void SetAIOrder(AIAreaController aiArea)
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
    public void SetOrder(OrderDataStruct singleOrder)
    {
        _orderList.Add(singleOrder);
    }
    private void NotifyOrderUpdates()
    {
        TableController.GivedOrderForAIWaiter?.Invoke(_table);

        if (ControllerManager.Instance._orderPanelController.OpenedTableNumber == _table.TableNumber)
        {
            Chair.GivedOrder?.Invoke(_table.TableNumber);
        }
    }
    public void RemoveOrder(OrderDataStruct orderDataStruct)
    {
        _orderList.Remove(orderDataStruct);
    }
    

}

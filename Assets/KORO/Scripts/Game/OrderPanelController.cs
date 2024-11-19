using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class OrderPanelController : MonoBehaviour
{
    [SerializeField] private GameObject _orderPanel;
    [SerializeField] private SingleOrder _singlePf;
    [SerializeField] private Transform _singlePfParent;
    [SerializeField] private Transform _singlePfParentForFoodList;
    [SerializeField] private Transform _singlePfParentForSelectedFoodList;
    [FormerlySerializedAs("_singleOrderList")] [SerializeField] private List<SingleOrder> _tableOrderList;
    public List<SingleOrder> SelectedOrderList;
    public List<SingleOrder> FoodList;
    public List<Orders> OrderList = new List<Orders>();

    public int OpenedTableNumber;
    
   

    public void Initialize()
    {
        Utilities.DeleteTransformchilds(_singlePfParentForSelectedFoodList);
        Utilities.DeleteTransformchilds(_singlePfParentForFoodList);
        for (int i = 0; i < GameDataManager.Instance.FoodDatas.Count; i++)
        {
            
            var singleOrder = Instantiate(_singlePf, _singlePfParentForFoodList);
            singleOrder.OrderType = GameDataManager.Instance.FoodDatas[i].OrderType;
            singleOrder.OrderPrice = GameDataManager.Instance.FoodDatas[i].OrderPrice;
            singleOrder.SingleOrderUIType = Enums.SingleOrderUIType.FoodList;
            FoodList.Add(singleOrder);
            singleOrder.Initialize();
        }
    }
    public void PlayerOrderInventory(SingleOrder singleOrder)
    {
        
        var singleOrderObject = Instantiate(_singlePf, _singlePfParentForSelectedFoodList);
        singleOrderObject.SingleOrderUIType = Enums.SingleOrderUIType.PlayerOrderList;
        singleOrderObject.OrderType = singleOrder.OrderType;
        singleOrderObject.OrderPrice = singleOrder.OrderPrice;
        singleOrderObject.Initialize();
        SelectedOrderList.Add(singleOrderObject);
        var DataStruct = new OrderDataStruct()
        {
            OrderType = singleOrder.OrderType,
            OrderPrice = singleOrder.OrderPrice,
        };
        PlayerOrderController.Instance.OrderList[OpenedTableNumber].OrderDataStructs.Add(DataStruct);
    }

    public void RemoveOrderFromLPayer(SingleOrder singleOrder)
    {
        SelectedOrderList.Remove(singleOrder);
        var openedOrderList = PlayerOrderController.Instance.OrderList[OpenedTableNumber];
        for (int i = 0; i < openedOrderList.OrderDataStructs.Count; i++)
        {
            if (openedOrderList.OrderDataStructs[i].OrderType == singleOrder.OrderType)
            {
                openedOrderList.OrderDataStructs.Remove(openedOrderList.OrderDataStructs[i]);
                Destroy(singleOrder.gameObject);
                return;
            }
        }
    }
    public void ShowOrder(List<OrderDataStruct> _orderList,int tableNumber)
    {
        SelectedOrderList.Clear();
        Utilities.DeleteTransformchilds(_singlePfParentForSelectedFoodList);
        OpenedTableNumber = tableNumber;
        CreateSelectedOrders();

        GameSceneCanvas.Instance.CanMove = false;
        GameSceneCanvas.IsCursorVisible?.Invoke(true);
        _orderPanel.gameObject.SetActive(true);
        CreateOrders(_orderList);
        if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 4)
        {
            TutorialManager.Instance.SetTutorialInfo(7);
        }
    }
    public void CreateSelectedOrders()
    {
        var orderList = PlayerOrderController.Instance.OrderList;
        for (int i = 0; i < orderList.Count; i++)
        {
            if (OpenedTableNumber == orderList[i].TableNumber)
            {
                for (int j = 0; j < orderList[i].OrderDataStructs.Count; j++)
                {
                    var order = Instantiate(_singlePf, _singlePfParentForSelectedFoodList);
                    order.OrderType = orderList[i].OrderDataStructs[j].OrderType;
                    order.Initialize();
                    _tableOrderList.Add(order);
                }
                
            }
            
        }
    }
    public void CanFollowTrue()
    {
        GameSceneCanvas.Instance.CanMove = true;
        GameSceneCanvas.IsCursorVisible?.Invoke(false);
        _orderPanel.gameObject.SetActive(false);
        OpenedTableNumber = -1;

    }

    
    public void CreateOrders(List<OrderDataStruct> _orderList)
    {
        Utilities.DeleteTransformchilds(_singlePfParent);
        _tableOrderList.Clear();
        for (int i = 0; i < _orderList.Count; i++)
        {
            var order = Instantiate(_singlePf, _singlePfParent);
            order.OrderType = _orderList[i].OrderType;
            order.Initialize();
            _tableOrderList.Add(order);
        }
    }

    
}

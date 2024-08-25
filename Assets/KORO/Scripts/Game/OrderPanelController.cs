using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class OrderPanelController : MonoBehaviour
{
    public static OrderPanelController Instance;
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
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void Initialize()
    {
        DeleteChilds(_singlePfParentForSelectedFoodList);
        DeleteChilds(_singlePfParentForFoodList);
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
        DeleteChilds(_singlePfParentForSelectedFoodList);
        OpenedTableNumber = tableNumber;
        CreateSelectedOrders();

        GameSceneCanvas.Instance.CanMove = false;
        _orderPanel.gameObject.SetActive(true);
        CreateOrders(_orderList);
        if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 4)
        {
            TutorialManager.Instance.SetTutorialInfo(6);
        }
    }
    public void CreateSelectedOrders()
    {
        for (int i = 0; i < PlayerOrderController.Instance.OrderList.Count; i++)
        {
            if (OpenedTableNumber == PlayerOrderController.Instance.OrderList[i].TableNumber)
            {
                for (int j = 0; j < PlayerOrderController.Instance.OrderList[i].OrderDataStructs.Count; j++)
                {
                    var order = Instantiate(_singlePf, _singlePfParentForSelectedFoodList);
                    order.OrderType = PlayerOrderController.Instance.OrderList[i].OrderDataStructs[j].OrderType;
                    order.Initialize();
                    _tableOrderList.Add(order);
                }
                
            }
            
        }
    }
    public void CanFollowTrue()
    {
        GameSceneCanvas.Instance.CanMove = true;
        _orderPanel.gameObject.SetActive(false);
        OpenedTableNumber = -1;

    }

    
    public void CreateOrders(List<OrderDataStruct> _orderList)
    {
        DeleteChilds(_singlePfParent);
        _tableOrderList.Clear();
        for (int i = 0; i < _orderList.Count; i++)
        {
            var order = Instantiate(_singlePf, _singlePfParent);
            order.OrderType = _orderList[i].OrderType;
            order.Initialize();
            _tableOrderList.Add(order);
        }
    }

    public void DeleteChilds(Transform parentTransform)
    {
        var orderArray = parentTransform.GetComponentsInChildren<SingleOrder>();
        if (orderArray.Length > 0)
        {
            for (int i = 0; i < orderArray.Length; i++)
            {
                Destroy(orderArray[i].gameObject);
            }
        }
    }
}

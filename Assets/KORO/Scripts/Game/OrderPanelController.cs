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

    private void Start()
    {
        DeleteChilds(_singlePfParentForSelectedFoodList);

        DeleteChilds(_singlePfParentForFoodList);
        for (int i = 0; i < GameDataManager.Instance.Orders.Count; i++)
        {
            
            var singleOrder = Instantiate(_singlePf, _singlePfParentForFoodList);
            singleOrder.OrderType = GameDataManager.Instance.Orders[i].OrderType;
            singleOrder.OrderPrice = GameDataManager.Instance.Orders[i].OrderPrice;
            singleOrder.SingleOrderUIType = Enums.SingleOrderUIType.FoodList;
            FoodList.Add(singleOrder);
            singleOrder.Initialize();
        }
    }
    public void PlayerOrderInventory(SingleOrder singleOrder)
    {
        
        var singleOrderObject = Instantiate(_singlePf, _singlePfParentForSelectedFoodList);
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
    public void ShowOrder(List<OrderDataStruct> _orderList,int tableNumber)
    {
        OpenedTableNumber = tableNumber;
        GameSceneCanvas.Instance.CanMove = false;
        _orderPanel.gameObject.SetActive(true);
        CreateOrders(_orderList);
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

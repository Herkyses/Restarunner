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
    [FormerlySerializedAs("_singleOrderList")] [SerializeField] private List<SingleOrder> _tableOrderList;
    [SerializeField] private List<SingleOrder> _orderList;
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
        DeleteChilds();
        _tableOrderList.Clear();
        for (int i = 0; i < _orderList.Count; i++)
        {
            var order = Instantiate(_singlePf, _singlePfParent);
            order.Initialize();
            _tableOrderList.Add(order);
        }
    }

    public void DeleteChilds()
    {
        var orderArray = _singlePfParent.GetComponentsInChildren<SingleOrder>();
        if (orderArray.Length > 0)
        {
            for (int i = 0; i < orderArray.Length; i++)
            {
                Destroy(orderArray[i].gameObject);
            }
        }
    }
}

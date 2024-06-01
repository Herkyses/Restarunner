using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Table : MonoBehaviour,IInterectableObject, IAIInteractable
{
    [SerializeField] private string checkOrder = "Check Order";
    [SerializeField] private List<OrderDataStruct> _orderList ;
    public bool IsTableAvailable ;
    public int TableNumber ;
    public int TableCapacity;
    public int CustomerCount;

    public List<Chair> ChairList;
    private void OnEnable()
    {
        Chair.GivedOrder += CreateOrdersWithAction;
    }

    private void OnDisable()
    {
        Chair.GivedOrder -= CreateOrdersWithAction;

    }
    public void StartState(Transform AITransform)
    {
        AvailabilityControl();
        var chair = CheckChairAvailable();
        if (chair)
        {
            chair.isChairAvailable = false;
            AITransform.position = chair.transform.position;
            AITransform.rotation = chair.transform.rotation;
            var stateMAchineController = AITransform.gameObject.GetComponent<AIStateMachineController>();
            stateMAchineController.AIChangeState(stateMAchineController.AISitState);
            AITransform.gameObject.GetComponent<AIAreaController>().InteractabelControl();
            var orderIndex = Random.Range(0, 2);
            SetOrderTable(orderIndex);
            if (OrderPanelController.Instance.OpenedTableNumber == TableNumber)
            {
                Chair.GivedOrder?.Invoke(TableNumber);
            }
        }
        
    }
    public void SetOrderTable(int orderIndex)
    {
        
        var newOrder = new OrderDataStruct()
        {
            OrderType = (Enums.OrderType) orderIndex,
        };
        SetOrder(newOrder);
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
    public void AvailabilityControl()
    {
        if (CustomerCount < TableCapacity)
        {
            CustomerCount++;
        }
        else
        {
            IsTableAvailable = false;
        }
    }

    private void Start()
    {
        IsTableAvailable = false;
    }

    public void CreateOrdersWithAction(int tableNumber)
    {
        if (tableNumber == TableNumber)
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

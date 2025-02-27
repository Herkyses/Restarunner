using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterController : MonoBehaviour
{
    [SerializeField] private List<OrderDataStruct> _orderList ;
    public int TableNumber ;
    public AIStateMachineController AIStateMachineController;
    public Table OwnerTableForWaiter;
    public ChefController ChefController;
    public TableBill TableBill;
    public List<FoodTable> FoodTable;
    public bool IsAvailable;


    private void Awake()
    {
        gameObject.TryGetComponent(out AIStateMachineController);
    }

    private void OnEnable()
    {
        ChefController.OnFoodCreated += WaiterMoveFoodStateStart;
    }

    private void OnDisable()
    {
        ChefController.OnFoodCreated -= WaiterMoveFoodStateStart;
    }
    
    public void WaiterMoveStateStart(Table table)
    {
        IsAvailable = false;
        OwnerTableForWaiter = table;
        AIStateMachineController.AITargetTableTransform = table.transform;
        AIStateMachineController.AIChangeState(AIStateMachineController.AIWaiterMoveState);
    }

    public void WaiterMoveFoodStateStart(WaiterController ownerWaiter)
    {
        AIStateMachineController.AIChangeState(AIStateMachineController.AIWaiterTakeFoodState);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddOrder(List<OrderDataStruct> orderDataStructList)
    {
        _orderList = orderDataStructList;
    }

    public List<OrderDataStruct> GetOrders()
    {
        return _orderList;
    }
}

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
    public Food Food;


    private void Awake()
    {
        gameObject.TryGetComponent(out AIStateMachineController);
    }

    private void OnEnable()
    {
        TableController.GivedOrderForAIWaiter += WaiterMoveStateStart; 
    }

    private void OnDisable()
    {
        TableController.GivedOrderForAIWaiter -= WaiterMoveStateStart; 
    }
    
    public void WaiterMoveStateStart(Table table)
    {
        OwnerTableForWaiter = table;
        AIStateMachineController.AITargetTableTransform = table.transform;
        AIStateMachineController.AIChangeState(AIStateMachineController.AIWaiterMoveState);
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllWaitersController : MonoBehaviour
{
    public List<WaiterController> WaiterControllers;
    private void OnEnable()
    {
        TableController.GivedOrderForAIWaiter += WaiterMoveStateStart; 
    }

    private void OnDisable()
    {
        TableController.GivedOrderForAIWaiter -= WaiterMoveStateStart; 
    }
    // Start is called before the first frame update
    public void WaiterMoveStateStart(Table table)
    {
        for (int i = 0; i < WaiterControllers.Count; i++)
        {
            if (WaiterControllers[i].IsAvailable)
            {
                WaiterControllers[i].OwnerTableForWaiter = table;
                WaiterControllers[i].AIStateMachineController.AITargetTableTransform = table.transform;
                WaiterControllers[i].AIStateMachineController.AIChangeState(WaiterControllers[i].AIStateMachineController.AIWaiterMoveState);
                return;
            }
        }
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

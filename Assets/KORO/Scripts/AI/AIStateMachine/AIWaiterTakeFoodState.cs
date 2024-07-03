using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWaiterTakeFoodState : AIBaseState
{
    public string StateName;
    public AIStateMachineController AIStateMachineController;
    public AIWaiterTakeFoodState(AIStateMachineController stateMachine) : base("AIWaiterTakeFoodState", stateMachine)
    {
        AIStateMachineController = stateMachine;
    }
    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        if (Vector3.Distance(AIStateMachineController.transform.position, TableController.Instance.ChefController.transform.position) < 1f)
        {
            
        }
        
    }

    public override void ExitState()
    {
        
        
    }
}

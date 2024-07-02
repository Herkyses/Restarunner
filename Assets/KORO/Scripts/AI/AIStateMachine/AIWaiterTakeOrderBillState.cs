using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWaiterTakeOrderBillState : AIBaseState
{
    public string StateName;
    public AIStateMachineController AIStateMachineController;
    public AIWaiterTakeOrderBillState(AIStateMachineController stateMachine) : base("AIWaiterTakeOrderBillState", stateMachine)
    {
        AIStateMachineController = stateMachine;
    }
    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        
        
    }

    public override void ExitState()
    {
        
        
    }
}

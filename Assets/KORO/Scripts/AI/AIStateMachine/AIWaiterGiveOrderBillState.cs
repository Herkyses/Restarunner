using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWaiterGiveOrderBillState : AIBaseState
{
    public string StateName;
    public AIStateMachineController AIStateMachineController;
    public AIWaiterGiveOrderBillState(AIStateMachineController stateMachine) : base("AIWaiterGiveOrderBillState", stateMachine)
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

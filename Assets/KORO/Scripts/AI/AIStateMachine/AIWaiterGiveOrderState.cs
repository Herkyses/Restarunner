using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWaiterGiveOrderState : AIBaseState
{
    public string StateName;
    public AIStateMachineController AIStateMachineController;
    public AIWaiterGiveOrderState(AIStateMachineController stateMachine) : base("AIWaiterGiveOrderState", stateMachine)
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

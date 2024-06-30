using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWaiterState : AIBaseState
{
    public string StateName;
    public AIStateMachineController AIStateMachineController;
    public AIWaiterState(AIStateMachineController stateMachine) : base("AIWaiterStates", stateMachine)
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

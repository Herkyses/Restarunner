using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveState : AIBaseState
{
    public string StateName;
    public AIStateMachineController AIStateMachineController;
    public AIMoveState(AIStateMachineController stateMachine) : base("AIMoveState", stateMachine)
    {
        AIStateMachineController = stateMachine;
    }

    public override void EnterState()
    {
        AIStateMachineController.AIController.StartTargetDestination();
    }

    public override void UpdateState()
    {
        
        
    }

    public override void ExitState()
    {
        
        
    }
}

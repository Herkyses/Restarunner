using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIClapState : AIBaseState
{
    public string StateName;
    public AIStateMachineController AIStateMachineController;
    public AIClapState(AIStateMachineController stateMachine) : base("AIClapState", stateMachine)
    {
        AIStateMachineController = stateMachine;
    }

    public override void EnterState()
    {
        AIStateMachineController.AIController.StartClapState();
        
    }

    public override void UpdateState()
    {
        
        
    }

    public override void ExitState()
    {
        
        
    }
}

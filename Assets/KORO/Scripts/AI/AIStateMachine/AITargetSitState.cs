using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITargetSitState : AIBaseState
{
    public string StateName;
    public AIStateMachineController AIStateMachineController;
    public AITargetSitState(AIStateMachineController stateMachine) : base("AITargetSitState", stateMachine)
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

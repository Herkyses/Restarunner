using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIBaseState
{
    public string StateName;
    public AIStateMachineController AIStateMachineController;
    public AIIdleState(AIStateMachineController stateMachine) : base("AIIdleState", stateMachine)
    {
        AIStateMachineController = stateMachine;
    }

    public override void EnterState()
    {
        AIStateMachineController.AIController.AIAnimationController.PlayIdleAnimation();
        AIStateMachineController.AIController._agent.speed = 0;

    }

    public override void UpdateState()
    {
        
        
    }

    public override void ExitState()
    {
        
        
    }
}

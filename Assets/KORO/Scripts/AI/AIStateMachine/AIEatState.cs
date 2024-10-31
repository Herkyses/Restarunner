using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEatState : AIBaseState
{
    public string StateName;
    public AIStateMachineController AIStateMachineController;
    public AIEatState(AIStateMachineController stateMachine) : base("AIEatState", stateMachine)
    {
        AIStateMachineController = stateMachine;
    }
    public override void EnterState()
    {
        //AIStateMachineController.AIController.StartSitState();
        Debug.Log("EAT State Enter");
        AIStateMachineController.AIAnimationController.PlayEatAnimation();
        AIStateMachineController.StartEatState();
    }

    public override void UpdateState()
    {
        
        
    }

    public override void ExitState()
    {
        
        
    }
}

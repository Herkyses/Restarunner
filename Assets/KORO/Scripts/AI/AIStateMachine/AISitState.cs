using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISitState : AIBaseState
{
    public string StateName;
    public AIStateMachineController AIStateMachineController;
    public AISitState(AIStateMachineController stateMachine) : base("AISitState", stateMachine)
    {
        AIStateMachineController = stateMachine;
    }
    
    public override void EnterState()
    {
        AIStateMachineController.AIController.StartSitState();
        
    }

    public override void UpdateState()
    {
        
        
    }

    public override void ExitState()
    {
        
        
    }
}

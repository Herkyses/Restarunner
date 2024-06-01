using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChefState : AIBaseState
{
    public string StateName;
    public AIStateMachineController AIStateMachineController;
    public AIChefState(AIStateMachineController stateMachine) : base("AIChefState", stateMachine)
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

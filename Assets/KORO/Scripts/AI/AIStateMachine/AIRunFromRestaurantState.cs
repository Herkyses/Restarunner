using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRunFromRestaurantState : AIBaseState
{
    public string StateName;
    public AIStateMachineController AIStateMachineController;
    
    public AIRunFromRestaurantState(AIStateMachineController stateMachine) : base("AIRunFromRestaurantState", stateMachine)
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

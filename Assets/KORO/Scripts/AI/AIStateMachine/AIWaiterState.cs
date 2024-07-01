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
        if (Vector3.Distance(AIStateMachineController.transform.position, AIStateMachineController.AITargetSitTransform.position) < 0.2f)
        {
            AIStateMachineController.AIController.GetComponent<AIAreaController>().StartInteractableObject(Enums.AIStateType.Waiter);
        }
        
    }

    public override void ExitState()
    {
        
        
    }
}

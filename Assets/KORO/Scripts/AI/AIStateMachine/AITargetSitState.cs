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
        AIStateMachineController.AIController.AIAnimationController.PlayMoveAnimation();
        AIStateMachineController.AIWaitTimeController.WaitTimeStarted = false;
    }

    public override void UpdateState()
    {
        if (Vector3.Distance(AIStateMachineController.transform.position, AIStateMachineController.OwnerTable.transform.position) < 1.2f)
        {
            //AIStateMachineController.AIController.GetComponent<AIAreaController>().StartInteractableObject(Enums.AIStateType.Customer);
            AIStateMachineController.OwnerTable.StartState(AIStateMachineController.AIController.GetComponent<AIAreaController>(),Enums.AIStateType.Customer);
        }
    }

    public override void ExitState()
    {
        
        
    }
}

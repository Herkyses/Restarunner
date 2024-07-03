using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWaiterMoveState : AIBaseState
{
    public string StateName;
    public AIStateMachineController AIStateMachineController;
    public AIWaiterMoveState(AIStateMachineController stateMachine) : base("AIWaiterMoveState", stateMachine)
    {
        AIStateMachineController = stateMachine;
    }
    public override void EnterState()
    {
        AIStateMachineController.AIController._agent.speed = 1;
        AIStateMachineController.AIController.AIAnimationController.PlayMoveAnimation();
        AIStateMachineController.AIController._agent.destination = AIStateMachineController.AITargetTableTransform.position;
    }

    public override void UpdateState()
    {
        if (Vector3.Distance(AIStateMachineController.transform.position, AIStateMachineController.AITargetTableTransform.position) < 0.5f)
        {
            AIStateMachineController.AIController.AIAnimationController.PlayIdleAnimation();
            AIStateMachineController.AIController._agent.speed = 0;
            //AIStateMachineController.AIController.GetComponent<AIAreaController>().StartInteractableObject(Enums.AIStateType.Waiter);
            AIStateMachineController.AIWaiterController.OwnerTableForWaiter.StartState(AIStateMachineController.AIAreaController,Enums.AIStateType.Waiter);
            AIStateMachineController.AIChangeState(AIStateMachineController.AIWaiterMoveChefState);
        }
        
    }

    public override void ExitState()
    {
        
        
    }
}

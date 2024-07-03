using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWaiterMoveChefState : AIBaseState
{
    public string StateName;
    public AIStateMachineController AIStateMachineController;
    public AIWaiterMoveChefState(AIStateMachineController stateMachine) : base("AIWaiterMoveChefState", stateMachine)
    {
        AIStateMachineController = stateMachine;
    }
    public override void EnterState()
    {
        AIStateMachineController.AIController._agent.destination = TableController.Instance.ChefController.transform.position;
        AIStateMachineController.AIController._agent.speed = 1;
        AIStateMachineController.AIController.AIAnimationController.PlayMoveAnimation();

    }

    public override void UpdateState()
    {
        if (Vector3.Distance(AIStateMachineController.transform.position, TableController.Instance.ChefController.transform.position) < 1f)
        {
            AIStateMachineController.AIController._agent.speed = 0;
            AIStateMachineController.AIChangeState(AIStateMachineController.AIWaiterGiveOrderState);
            AIStateMachineController.AIController.AIAnimationController.PlayIdleAnimation();

            //AIStateMachineController.AIController.GetComponent<AIAreaController>().StartInteractableObject(Enums.AIStateType.Waiter);
            //AIStateMachineController.AIWaiterController.OwnerTableForWaiter.StartState(AIStateMachineController.AIAreaController,Enums.AIStateType.Waiter);
        }
        
    }

    public override void ExitState()
    {
        
        
    }
}

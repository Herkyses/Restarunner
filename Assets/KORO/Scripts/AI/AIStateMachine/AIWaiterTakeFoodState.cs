using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWaiterTakeFoodState : AIBaseState
{
    public string StateName;
    public AIStateMachineController AIStateMachineController;
    public AIWaiterTakeFoodState(AIStateMachineController stateMachine) : base("AIWaiterTakeFoodState", stateMachine)
    {
        AIStateMachineController = stateMachine;
    }
    public override void EnterState()
    {
        AIStateMachineController.AIController._agent.destination = AIStateMachineController.AIWaiterController.FoodTable[0].transform.position;
        AIStateMachineController.AIController.AIAnimationController.PlayMoveAnimation();

    }

    public override void UpdateState()
    {
        if (Vector3.Distance(AIStateMachineController.transform.position, AIStateMachineController.AIWaiterController.FoodTable[0].transform.position) < 1f)
        {
            AIStateMachineController.AIController._agent.speed = 0;
            AIStateMachineController.AIWaiterController.FoodTable[0].transform.SetParent(AIStateMachineController.transform);
            AIStateMachineController.AIChangeState(AIStateMachineController.AIWaiterGiveFoodState);
            AIStateMachineController.AIController.AIAnimationController.PlayIdleAnimation();

        }
        
    }

    public override void ExitState()
    {
        
        
    }
}

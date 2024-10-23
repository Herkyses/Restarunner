using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITargetRestaurantState : AIBaseState
{
    
    public string StateName;
    public AIStateMachineController AIStateMachineController;
    public AITargetRestaurantState(AIStateMachineController stateMachine) : base("AITargetRestaurantState", stateMachine)
    {
        AIStateMachineController = stateMachine;
    }

    public override void EnterState()
    {
        //AIStateMachineController.AIController.StartTargetDestination();
        AIStateMachineController.AIController._agent.destination = AIWaitStateController.Instance.transform.position;
        AIStateMachineController.AIAnimationController.PlayMoveAnimation();

    }

    public override void UpdateState()
    {
        if (Vector3.Distance(AIStateMachineController.transform.position, AIWaitStateController.Instance.transform.position) < 1f)
        {
            if (PlaceController.Instance.RestaurantIsOpen)
            {
                AIStateMachineController.AIChangeState(AIStateMachineController.AIWaitPlayerState);
            }
            else
            {
                AIStateMachineController.AIChangeState(AIStateMachineController.AIMoveState);

            }
            
        }
        
    }

    public override void ExitState()
    {
        
        
    }
}

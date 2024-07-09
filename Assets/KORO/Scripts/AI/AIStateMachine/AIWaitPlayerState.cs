using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWaitPlayerState : AIBaseState
{
    public string StateName;
    public AIStateMachineController AIStateMachineController;
    public AIWaitPlayerState(AIStateMachineController stateMachine) : base("AIWaitPlayerState", stateMachine)
    {
        AIStateMachineController = stateMachine;
    }

    public override void EnterState()
    {
        AIStateMachineController.AIController.AIAnimationController.PlayIdleAnimation();
        AIStateMachineController.AIController._agent.speed = 0;

        var AIWaitStateController = Places.Instance.DoorTransform.gameObject.GetComponent<AIWaitStateController>();
        AIWaitStateController.AddList(AIStateMachineController.AIController);
        AIStateMachineController.AIWaitTimeController.WaitTimeValue = 10f;
        AIStateMachineController.AIWaitTimeController.WaitTimeStarted = true;
        AISpawnController.Instance.CreateAIForGroup(AIStateMachineController.Friends,AIStateMachineController.transform);
    }
    
    public override void UpdateState()
    {
        
        /*if (!PlaceController.RestaurantIsOpen)
        {
            AIStateMachineController.AIChangeState(AIStateMachineController.AIMoveState);

        }*/
    }

    public override void ExitState()
    {
        
        
    }
}

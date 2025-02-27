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

        var AIWaitStateController = global::AIWaitStateController.Instance;
        AIStateMachineController.AIWaitTimeController.WaitTimeValue = 10f;
        AIStateMachineController.AIWaitTimeController.WaitTimeStarted = true;
        AISpawnController.Instance.CreateAIForGroup(AIStateMachineController.Friends,AIStateMachineController.transform);
        AIWaitStateController.AddList(AIStateMachineController.AIController,AIStateMachineController.Friends.Count);

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

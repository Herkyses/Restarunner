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
        Collider[] colliders = Physics.OverlapSphere(AIStateMachineController.transform.position, 0.3f);
        
        foreach (Collider col in colliders)
        {
            if (col.gameObject.GetComponent<AIWaitStateController>())
            {
                var AIWaitStateController = col.gameObject.GetComponent<AIWaitStateController>();
                AIWaitStateController.AddList(AIStateMachineController.AIController);
            }
            else
            {
                
            }
        }
    }

    public override void UpdateState()
    {
        
        
    }

    public override void ExitState()
    {
        
        
    }
}

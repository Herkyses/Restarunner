using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveState : AIBaseState
{
    public string StateName;
    public AIStateMachineController AIStateMachineController;
    public AIMoveState(AIStateMachineController stateMachine) : base("AIMoveState", stateMachine)
    {
        AIStateMachineController = stateMachine;
    }

    public override void EnterState()
    {
        AIStateMachineController.AIController.StartTargetDestination(AIStateMachineController.AIController._targetPositions[2].position);
    }

    public override void UpdateState()
    {
        if (Vector3.Distance(AIStateMachineController.transform.position, AIStateMachineController.AIController.TargetTransform.position) < 1f)
        {
            AISpawnController.Instance.SetTransformForAI(AIStateMachineController.AIController);
        }
        
    }

    public override void ExitState()
    {
        
        
    }
}

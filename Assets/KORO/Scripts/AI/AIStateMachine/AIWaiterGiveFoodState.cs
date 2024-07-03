using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWaiterGiveFoodState : AIBaseState
{
    public string StateName;
    public bool isFinish;
    public AIStateMachineController AIStateMachineController;
    public AIWaiterGiveFoodState(AIStateMachineController stateMachine) : base("AIWaiterGiveFoodState", stateMachine)
    {
        AIStateMachineController = stateMachine;
    }
    public override void EnterState()
    {
        AIStateMachineController.AIController._agent.destination = AIStateMachineController.AIWaiterController.OwnerTableForWaiter.transform.position;
        AIStateMachineController.AIController._agent.speed = 1f;
    }

    public override void UpdateState()
    {
        if (!isFinish)
        {
            if (Vector3.Distance(AIStateMachineController.transform.position, AIStateMachineController.AIWaiterController.OwnerTableForWaiter.transform.position) < 1f)
            {
                AIStateMachineController.AIWaiterController.FoodTable.Remove(AIStateMachineController.AIWaiterController.FoodTable[0]);
            
                if (AIStateMachineController.AIWaiterController.FoodTable.Count > 0)
                {
                    AIStateMachineController.AIChangeState(AIStateMachineController.AIWaiterTakeFoodState);
                }
                else
                {
                    isFinish = true;
                }
            }
        }
        
        
    }

    public override void ExitState()
    {
        
        
    }
}

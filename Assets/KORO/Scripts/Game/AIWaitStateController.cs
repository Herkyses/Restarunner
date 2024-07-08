using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWaitStateController : MonoBehaviour,IInterectableObject
{
    public List<AIController> AiControllers;
    public static AIWaitStateController Instance;
    
        
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    
    public void InterectableObjectRun()
    {
        TableAvailablePanel.Instance.ActiveAbilityPanel();
    }

    public void ShowOutline(bool active)
    {
            
    }

    public void AddList(AIController aiController)
    {
        AiControllers.Add(aiController);
        TableAvailablePanel.Instance.SetCustomerList(aiController.AgentID);
    }

    public void RemoveFromAiControllersList(AIController aiController)
    {
        AiControllers.Remove(aiController);
    }
    public Outline GetOutlineComponent()
    {
        return null;
    }

    public string GetInterectableText()
    {
        return "CheckAvailableTables";
    }

    public void AISetTablePos(int index,Table table)
    {
        for (int i = 0; i < AiControllers.Count; i++)
        {
            if (AiControllers[i].AgentID == index)
            {
                AiControllers[i].AIStateMachineController.AITargetSitTransform = table.transform;
                AiControllers[i].AIStateMachineController.OwnerTable = table;
                AiControllers[i].AIStateMachineController.AIChangeState(AiControllers[i].AIStateMachineController.AITargetSitState);
                AiControllers[i]._agent.destination = table.transform.position;
                AiControllers[i]._agent.speed = 1f;
                TableAvailablePanel.Instance.RemoveFromCustomerList(index);
            }
        }
    }
    public void Move()
    {
        
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWaitStateController : MonoBehaviour,IInterectableObject
{
    public List<AIController> AiControllers;
    
    
        
    
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
        TableAvailablePanel.Instance.SetCustomerList();
    }
    public Outline GetOutlineComponent()
    {
        return null;
    }

    public string GetInterectableText()
    {
        return "CheckAvailableTables";
    }

    
}

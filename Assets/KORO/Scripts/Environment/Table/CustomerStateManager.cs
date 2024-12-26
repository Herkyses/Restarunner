using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerStateManager : MonoBehaviour
{
    private Table _table;
    public List<Chair> ChairList;
    public List<AIController> _aiControllerList ;
    [SerializeField] private OrderHandler _orderHandler;
    

    public void StartState(AIAreaController aiArea, Table table)
    {
        _table = table;  ///////geçiciiiiii
        AssignAIToChairAndOrder(aiArea);
        _orderHandler.NotifyOrderUpdates();
    }
    public bool CheckAllCustomerFinishedFood() =>
        _aiControllerList.Count > 0 && _aiControllerList.TrueForAll(ai => ai.IsFinishedFood);
    
    private void AssignAIToChairAndOrder(AIAreaController aiArea)
    {
        var chair = CheckChairAvailable();
        if (chair == null) return;

        AssignAIToChair(aiArea, chair);
        _orderHandler.SetAIOrder(aiArea);
    }
    public Chair CheckChairAvailable()
    {
        for (int i = 0; i < ChairList.Count; i++)
        {
            if (ChairList[i].isChairAvailable)
            {
                return ChairList[i];
            }
        }

        return null;
    }
    
    private void AssignAIToChair(AIAreaController aiArea, Chair chair)
    {
        chair.isChairAvailable = false;
        aiArea.AIController.AssignToChair(aiArea,chair.transform);
        _aiControllerList.Add(aiArea.GetComponent<AIController>());
        aiArea.AIController.SetTableInfo(_table, chair);
    }
    
   
    

}

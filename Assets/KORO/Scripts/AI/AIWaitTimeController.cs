using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWaitTimeController : MonoBehaviour
{
    public bool WaitTimeStarted;
    public float WaitTimeValue;
    public int WaitTimeTempValue;
    public AIStateMachineController AIStateMachineController;

    public Coroutine WaitCoroutine;

    private void Start()
    {
        gameObject.TryGetComponent(out AIStateMachineController);
    }

    // Update is called once per frame
    void Update()
    {
        if (!WaitTimeStarted)
        {
            return;
        }
       
        if (WaitTimeValue > 0)
        {
            WaitTimeValue -= Time.deltaTime;

            WaitTimeTempValue = (int) WaitTimeValue;
        }
        else
        {
            WaitTimeStarted = false;
            AIStateMachineController.AIChangeState(AIStateMachineController.AIMoveState);
            if (AIStateMachineController.Friends.Count > 0)
            {
                for (int i = 0; i < AIStateMachineController.Friends.Count; i++)
                {
                    //PoolManager.Instance.ReturnToPoolForCustomerAI(AIStateMachineController.Friends[i].gameObject);
                    PoolManager.Instance.ReturnToPoolForRagdollCustomerAI(AIStateMachineController.Friends[i].gameObject);
                }
                AIStateMachineController.Friends.Clear();
            }
            TableAvailablePanel.Instance.RemoveFromCustomerList(AIStateMachineController.AIController.AgentID);
            AIWaitStateController.Instance.RemoveFromAiControllersList(AIStateMachineController.AIController);
        }
        
    }

    public IEnumerator StartWaitTime()
    {
        yield return new WaitForSeconds(10f);
        WaitTimeStarted = false;
        AIStateMachineController.AIChangeState(AIStateMachineController.AIMoveState);
        if (AIStateMachineController.Friends.Count > 0)
        {
            for (int i = 0; i < AIStateMachineController.Friends.Count; i++)
            {
                //PoolManager.Instance.ReturnToPoolForCustomerAI(AIStateMachineController.Friends[i].gameObject);
                PoolManager.Instance.ReturnToPoolForRagdollCustomerAI(AIStateMachineController.Friends[i].gameObject);
            }
            AIStateMachineController.Friends.Clear();
        }
        TableAvailablePanel.Instance.RemoveFromCustomerList(AIStateMachineController.AIController.AgentID);
        AIWaitStateController.Instance.RemoveFromAiControllersList(AIStateMachineController.AIController);
        
    }

    public void StartWaitTimeCoroutine()
    {
        if (WaitCoroutine != null)
        {
            StopCoroutine(WaitCoroutine);
        }
        WaitCoroutine = StartCoroutine(StartWaitTime());
    }

    public void StopWaitTimeCoroutine()
    {
        
    }

    public void TimeCountStarted(float waitTime)
    {
        WaitTimeStarted = true;
        WaitTimeValue = waitTime;
    }
}

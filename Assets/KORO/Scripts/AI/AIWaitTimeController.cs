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


    private void Start()
    {
        gameObject.TryGetComponent(out AIStateMachineController);
    }

    // Update is called once per frame
    void Update()
    {
        if (WaitTimeStarted)
        {
            if (WaitTimeValue > 0)
            {
                WaitTimeValue -= Time.deltaTime;
                if ((int)WaitTimeTempValue != (int)WaitTimeValue)
                {
                    Debug.Log("timeendededed : " +(int)WaitTimeValue);

                }

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
                        AIStateMachineController.Friends[i].AIStateMachineController.AIChangeState(AIStateMachineController.Friends[i].AIStateMachineController.AIMoveState);
                    }
                    AIStateMachineController.Friends.Clear();
                }
                TableAvailablePanel.Instance.RemoveFromCustomerList(AIStateMachineController.AIController.AgentID);
                AIWaitStateController.Instance.RemoveFromAiControllersList(AIStateMachineController.AIController);
            }
        }
    }

    public void TimeCountStarted(float waitTime)
    {
        WaitTimeStarted = true;
        WaitTimeValue = waitTime;
    }
}

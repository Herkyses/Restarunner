using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWaitTimeController : MonoBehaviour
{
    public bool WaitTimeStarted;
    public float WaitTimeValue;
    public int WaitTimeTempValue;
    
    
    

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
            }
        }
    }

    public void TimeCountStarted(float waitTime)
    {
        WaitTimeStarted = true;
        WaitTimeValue = waitTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBaseState 
{
    public string StateName;
    public AIStateMachineController AIStateMachineController;
    public AIBaseState(string name, AIStateMachineController aiStateMachine)
    {
        this.StateName = name;
        this.AIStateMachineController = aiStateMachine;
    }
    public virtual void EnterState(){}
    public virtual void UpdateState(){}
    public virtual void ExitState(){}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAIInteractable
{
    public void StartState(AIAreaController AIArea,Enums.AIStateType aiStateType);
}

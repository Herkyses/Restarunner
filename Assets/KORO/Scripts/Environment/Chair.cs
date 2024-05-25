using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour, IAIInteractable
{
    [SerializeField] private Table _ownerTable;
    public void StartState(Transform AITransform)
    {
        AITransform.position = transform.position;
        AITransform.rotation = transform.rotation;
        var stateMAchineController = AITransform.gameObject.GetComponent<AIStateMachineController>();
        stateMAchineController.AIChangeState(stateMAchineController.AISitState);
        AITransform.gameObject.GetComponent<AIAreaController>().InteractabelControl();
    }

    public void SetOrderTable()
    {
        
    }

}

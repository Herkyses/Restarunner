using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chair : MonoBehaviour, IAIInteractable
{
    [SerializeField] private Table _ownerTable;
    [SerializeField] private int _tableNumber;
    public static Action<int> GivedOrder;
    public bool isChairAvailable;

    private void Start()
    {
        _tableNumber = _ownerTable.TableNumber;
        isChairAvailable = true;
    }

    public void StartState(Transform AITransform)
    {
        /*_ownerTable.AvailabilityControl();
        _tableNumber = _ownerTable.TableNumber;
        AITransform.position = transform.position;
        AITransform.rotation = transform.rotation;
        var stateMAchineController = AITransform.gameObject.GetComponent<AIStateMachineController>();
        stateMAchineController.AIChangeState(stateMAchineController.AISitState);
        AITransform.gameObject.GetComponent<AIAreaController>().InteractabelControl();
        var orderIndex = Random.Range(0, 2);
        SetOrderTable(orderIndex);
        if (OrderPanelController.Instance.OpenedTableNumber == _tableNumber)
        {
            GivedOrder?.Invoke(_tableNumber);

        }*/
    }

    

}

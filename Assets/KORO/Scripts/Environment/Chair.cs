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

    private void Start()
    {
        _tableNumber = _ownerTable.TableNumber;
    }

    public void StartState(Transform AITransform)
    {
        _tableNumber = _ownerTable.TableNumber;
        AITransform.position = transform.position;
        AITransform.rotation = transform.rotation;
        var stateMAchineController = AITransform.gameObject.GetComponent<AIStateMachineController>();
        stateMAchineController.AIChangeState(stateMAchineController.AISitState);
        AITransform.gameObject.GetComponent<AIAreaController>().InteractabelControl();
        var orderIndex = Random.Range(0, 2);
        SetOrderTable(orderIndex);
        GivedOrder?.Invoke(_tableNumber);
    }

    public void SetOrderTable(int orderIndex)
    {
        
        var newOrder = new OrderDataStruct()
        {
           OrderType = (Enums.OrderType) orderIndex,
        };
        _ownerTable.SetOrder(newOrder);
    }

}

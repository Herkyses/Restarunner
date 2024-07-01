using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chair : MonoBehaviour
{
    [SerializeField] private Table _ownerTable;
    [SerializeField] private int _tableNumber;
    public static Action<int> GivedOrder;
    public bool isChairAvailable;
    public Transform ChairFoodTransform;

    private void Start()
    {
        _tableNumber = _ownerTable.TableNumber;
        isChairAvailable = true;
    }

    

    

}

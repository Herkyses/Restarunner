using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public struct OrderDataStruct 
{
    public Enums.OrderType OrderType;
    public float OrderPrice;
}

[System.Serializable]

public struct Orders
{
    public int TableNumber;
    public List<OrderDataStruct> OrderDataStructs;
}

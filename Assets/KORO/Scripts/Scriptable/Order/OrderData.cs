using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderData", menuName = "Order/OrderData")]

public class OrderData : ScriptableObject
{
    public Enums.OrderType OrderType;
    public float OrderPrice;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData")]

public class FoodData : ScriptableObject
{
    public Enums.OrderType OrderDataStruct;
    public float OrderCost;
}

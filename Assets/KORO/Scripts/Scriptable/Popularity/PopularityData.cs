using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PopularityData", menuName = "Popularity/PopularityData")]

public class PopularityData : ScriptableObject
{
    public float FoodQualityMultiplier;
    public float CleanlinessMultiplier;
    public float CatchUpTimeMultiplier;
    public float DecorationQualityMultiplier;
    public float TableQualityMultiplier;
    public float CustomerPatienceMultiplier;
}

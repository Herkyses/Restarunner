using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Customers/CustomerData")]

public class CustomerData : ScriptableObject
{
    public List<Mesh> CharacterModels;
}

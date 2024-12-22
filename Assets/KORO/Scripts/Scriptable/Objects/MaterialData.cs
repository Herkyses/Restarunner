using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialData", menuName = "Material/MaterialData")]

public class MaterialData : ScriptableObject
{
    public Material TrueMaterial;
    public Material WrongMaterial;
}

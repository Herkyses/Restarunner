using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TableSetData", menuName = "TableSet/TableSetData")]
public class TableSetData : ScriptableObject
{
    public string[] texts;
    public string[] textsForTable;
    public string[] textsButtons;
    public string[] textsButtonsForTable;
    public string checkOrder;
}

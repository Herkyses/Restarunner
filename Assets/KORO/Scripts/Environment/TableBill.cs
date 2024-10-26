using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TableBill : MonoBehaviour,IInterectableObject
{
    public float BillValue;
    public Table OwnerTable;
    public TextMeshProUGUI OwnerTableNumberText;
    
    public void InterectableObjectRun()
    {
        PlayerOrderController.Instance.TakeBill(GetComponent<TableBill>());
    }

    public void Initiliaze(Table table ,float billValue)
    {
        OwnerTable = table;
        BillValue = billValue;
        OwnerTableNumberText.text = billValue.ToString();
    }

    public void ShowOutline(bool active)
    {
        
    }

    public Outline GetOutlineComponent()
    {
        return null;
    }

    public string GetInterectableText()
    {
        return "Take Order Bill";
    }
    public void Move()
    {
        
    }
    public void Open()
    {
        
    }
    public string[] GetInterectableTexts()
    {
        return null;
    }
    public string[] GetInterectableButtons()
    {
        return null;
    }
    public Enums.PlayerStateType GetStateType()
    {
        return Enums.PlayerStateType.Free;
    }
}

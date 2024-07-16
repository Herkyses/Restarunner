using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBill : MonoBehaviour,IInterectableObject
{
    public float BillValue;
    public Table OwnerTable;
    
    public void InterectableObjectRun()
    {
        PlayerOrderController.Instance.TakeBill(GetComponent<TableBill>());
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
}

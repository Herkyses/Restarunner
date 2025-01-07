using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TableBill : MonoBehaviour,IInterectableObject
{
    public float BillValue;
    public Table OwnerTable;
    public TextMeshProUGUI OwnerTableNumberText;
    
    [SerializeField] private string[] interactionTexts = new[] { "Key_OrderBill" };
    [SerializeField] private string[] interactionButtons = new[] { "E" };
    
    public void InterectableObjectRun()
    {
        PlayerOrderController.Instance.TakeBill(GetComponent<TableBill>());
    }

    public void Initiliaze(Table table ,float billValue)
    {
        OwnerTable = table;
        BillValue = billValue;
        OwnerTableNumberText.text = billValue.ToString();
        interactionTexts = new[] { "Key_OrderBill" };
        interactionButtons = new[] { "E" };
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

    public void Open()
    {
        
    }
    public string[] GetInterectableTexts()
    {
        return interactionTexts;
    }
    public string[] GetInterectableButtons()
    {
        return interactionButtons;
    }
    public Enums.PlayerStateType GetStateType()
    {
        return Enums.PlayerStateType.Free;
    }
}

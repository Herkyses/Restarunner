using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReservationTable : MonoBehaviour,IInterectableObject
{
    public void InterectableObjectRun()
    {
        TableAvailablePanel.Instance.ActiveAbilityPanel();

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
        return "CheckAvailableTables";
    }

    public void Move()
    {
        
    }

    public void Open()
    {
        
    }
}

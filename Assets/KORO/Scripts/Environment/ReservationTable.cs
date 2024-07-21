using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReservationTable : MonoBehaviour,IInterectableObject
{
    [SerializeField] private string[] texts = new [] {"Check Available "};
    [SerializeField] private string[] textsButtons = new [] {"E"};


    private void Start()
    {
        texts = new []{"Check Available "};
        textsButtons = new []{"E"};
    }

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
    public string[] GetInterectableTexts()
    {
        return texts;
    }
    public string[] GetInterectableButtons()
    {
        return textsButtons;
    }
}

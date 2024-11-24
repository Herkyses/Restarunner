using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReservationTable : MonoBehaviour,IInterectableObject
{
    [SerializeField] private string[] texts = new [] {"Key_Check_SuitableTable"};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    [SerializeField] private Outline outline;


    private void Start()
    {
        texts = new []{"Key_Check_SuitableTable"};
        textsButtons = new []{"E"};
        outline = GetComponent<Outline>();
    }

    public void InterectableObjectRun()
    {
        ControllerManager.Instance.TableAvailablePanel.ActiveAbilityPanel();

    }

    public void ShowOutline(bool active)
    {
        outline.enabled = active;
    }

    public Outline GetOutlineComponent()
    {
        return outline;
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
    public Enums.PlayerStateType GetStateType()
    {
        return Enums.PlayerStateType.Free;
    }
}

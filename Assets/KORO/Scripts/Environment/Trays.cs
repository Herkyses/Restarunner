using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trays : MonoBehaviour,IInterectableObject
{
    
    [SerializeField] private string[] texts = new [] {"Take OrderBox"};
    [SerializeField] private string[] textsButtons = new [] {"E"};


    private void Start()
    {
        texts = new [] {"Place the empty tray."};
        textsButtons = new [] {"E"};
    }

    public void InterectableObjectRun()
    {
        if (PlayerOrderController.Instance.FoodTable.gameObject)
        {
            PoolManager.Instance.ReturnToPoolForTrays(PlayerOrderController.Instance.FoodTable.gameObject);
            Player.Instance.PlayerStateType = Enums.PlayerStateType.Free;
        }
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
        return "Place the empty tray.";
    }
    public string[] GetInterectableTexts()
    {
        return texts;
    }
    public string[] GetInterectableButtons()
    {
        return textsButtons;
    }

    public void Move()
    {
        
    }
    public void Open()
    {
        
    }
    public Enums.PlayerStateType GetStateType()
    {
        return Enums.PlayerStateType.Trays;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubbish : MonoBehaviour,IInterectableObject
{
    
    [SerializeField] private string[] texts = new [] {"Take OrderBox"};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    private Outline _rubbishOutline;
    public void InterectableObjectRun()
    {
        
        //if (Player.Instance.CanCleanRubbish)
        //{
            gameObject.SetActive(false);
            Player.Instance.StartClean();
        //}
    }

    private void Start()
    {
        texts = new [] {"Clean"};
        textsButtons = new [] {"E"};
        _rubbishOutline = GetComponent<Outline>();
    }

    public void ShowOutline(bool active)
    {
        _rubbishOutline.enabled = active;
    }

    public Outline GetOutlineComponent()
    {
        return _rubbishOutline;
    }

    public string GetInterectableText()
    {
        if (Player.Instance.CanCleanRubbish)
        {
            return "Clean Rubbish";
        }
        return null;

    }

    public void Move()
    {
        
    }
    public void Open()
    {
        
    }
    public string[] GetInterectableTexts()
    {
        //if (Player.Instance.CanCleanRubbish)
        //{
            return texts;
        //}
        //return null;
    }
    public string[] GetInterectableButtons()
    {
        //if (Player.Instance.CanCleanRubbish)
        //{
            return textsButtons;
        //}
        //return null;
    }
    public Enums.PlayerStateType GetStateType()
    {
        return Enums.PlayerStateType.Cleaner;
    }
}

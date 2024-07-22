using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubbish : MonoBehaviour,IInterectableObject
{
    
    [SerializeField] private string[] texts = new [] {"Take OrderBox"};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    public void InterectableObjectRun()
    {
        
        if (Player.Instance.CanCleanRubbish)
        {
            CameraController.Instance.MoveCleanTool();
            gameObject.SetActive(false);
            if (RubbishManager.Instance.CheckRubbishLevel())
            {
                RubbishManager.Instance.UpdateRubbishLevel();
                RubbishManager.Instance.ActivateRubbishes();
            }        
        }
    }

    private void Start()
    {
        texts = new [] {"Clean"};
        textsButtons = new [] {"E"};
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
        if (Player.Instance.CanCleanRubbish)
        {
            return texts;
        }
        return null;
    }
    public string[] GetInterectableButtons()
    {
        if (Player.Instance.CanCleanRubbish)
        {
            return textsButtons;
        }
        return null;
    }
    public Enums.PlayerStateType GetStateType()
    {
        return Enums.PlayerStateType.Cleaner;
    }
}

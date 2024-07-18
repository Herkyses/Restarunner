using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubbish : MonoBehaviour,IInterectableObject
{
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
}

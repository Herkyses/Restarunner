using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseController : MonoBehaviour,IInterectableObject
{
    public void InterectableObjectRun()
    {
        PlaceController.RestaurantIsOpen = !PlaceController.RestaurantIsOpen;
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
        if (PlaceController.RestaurantIsOpen)
        {
            return "Close Restaurant";
        }
        else
        {
            return "Open Restaurant";

        }
    }

    public void Move()
    {
        
    }
}

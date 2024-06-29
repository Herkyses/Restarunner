using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceController : MonoBehaviour,IInterectableObject
{
    public static bool RestaurantIsOpen;

    public List<PlaceLevel> PlaceLevels;

    public int LevelValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        for (int i = 0; i < LevelValue; i++)
        {
            for (int j = 0; j < PlaceLevels[i].ActiveObject.Count; j++)
            {
                PlaceLevels[i].ActiveObject[j].SetActive(true);
                PlaceLevels[i].DeActiveObject[j].SetActive(false);
            }
        }   
    }
    
    public void InterectableObjectRun()
    {
        PlacePanelController.Instance.ActivePlacePanel();
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
        return "Check Place";
    }
    public void Move()
    {
        
    }
}

[System.Serializable]
public struct PlaceLevel
{
    public List<GameObject> ActiveObject;
    public List<GameObject> DeActiveObject;
}

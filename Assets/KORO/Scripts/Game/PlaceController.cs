using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceController : MonoBehaviour,IInterectableObject
{

    public bool RestaurantIsOpen;

    public List<PlaceLevel> PlaceLevels;
    private Outline _pcOutline;
    
    public Transform DoorTransform;

    [SerializeField] private string[] texts = new [] {"Take OrderBox"};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    [SerializeField] private GameObject decorationPlane;
    public int LevelValue;
    // Start is called before the first frame update
    void Start()
    {
        texts = new [] {"Use Computer"};
        textsButtons = new [] {"E"};
        _pcOutline = GetComponent<Outline>();

    }


    public void Initialize()
    {
        var levelValue = PlayerPrefsManager.Instance.LoadPlaceLevel();
        for (int i = 0; i < levelValue; i++)
        {
            for (int j = 0; j < PlaceLevels[i].ActiveObject.Count; j++)
            {
                PlaceLevels[i].ActiveObject[j].SetActive(true);
            }

            for (int j = 0; j < PlaceLevels[i].DeActiveObject.Count; j++)
            {
                PlaceLevels[i].DeActiveObject[j].SetActive(false);
            }
        }

        RestaurantIsOpen = false;
    }

    public void ActivateDecorationPlane(bool active)
    {
        decorationPlane.GetComponent<MeshRenderer>().enabled = active;
    }
    
    public void InterectableObjectRun()
    {
        if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() > 0)
        {
            ControllerManager.Instance.PlacePanelController.ActivePlacePanel();
        }
    }
    public void ShowOutline(bool active)
    {
        //_pcOutline.enabled = active;
    }
    public Outline GetOutlineComponent()
    {
        return _pcOutline;
    }
    public string GetInterectableText()
    {
        return "Use Computer";
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

[System.Serializable]
public struct PlaceLevel
{
    public List<GameObject> ActiveObject;
    public List<GameObject> DeActiveObject;
}

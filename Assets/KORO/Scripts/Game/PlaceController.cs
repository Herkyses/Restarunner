using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceController : MonoBehaviour,IInterectableObject
{
    public static PlaceController Instance;

    public static bool RestaurantIsOpen;

    public List<PlaceLevel> PlaceLevels;

    [SerializeField] private string[] texts = new [] {"Take OrderBox"};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    public int LevelValue;
    // Start is called before the first frame update
    void Start()
    {
        texts = new [] {"Use Computer"};
        textsButtons = new [] {"E"};
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
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
    }
    
    public void InterectableObjectRun()
    {
        if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() > 0)
        {
            PlacePanelController.Instance.ActivePlacePanel();
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

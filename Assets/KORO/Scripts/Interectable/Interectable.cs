using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interectable : MonoBehaviour,IInterectableObject
{
    [SerializeField] private Outline _ownerOutline;
    public string InterectableInfoText;
    public float RotationFirstValue;

    public EnvironmentInfo EnvironmentInfo;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.TryGetComponent(out _ownerOutline);
        RotationFirstValue = transform.localRotation.eulerAngles.y;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InterectableObjectRun()
    {
        var yValue = transform.localRotation.eulerAngles.y;
        if (yValue > 1)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0f,0,0f));
        }
        else
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0f,RotationFirstValue,0f));

        }
        /*if (transform.eulerAngles.y < 100)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0f,112f,0f));
            
        }
        
        else if (transform.eulerAngles.y > 100)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0f,0f,0f));

        }*/
    }
    public void ShowOutline(bool active)
    {
        _ownerOutline.enabled = active;
    }

    public Outline GetOutlineComponent()
    {
        return _ownerOutline;
    }

    public string GetInterectableText()
    {
        return EnvironmentInfo.CommandText;
    }
    public void Move()
    {
        
    }
    public void Open()
    {
        
    }
    public string[] GetInterectableTexts()
    {
        return null;
    }
    public string[] GetInterectableButtons()
    {
        return null;
    }
    public Enums.PlayerStateType GetStateType()
    {
        return Enums.PlayerStateType.GiveFood;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interectable : MonoBehaviour,IInterectableObject
{
    [SerializeField] private Outline _ownerOutline;
    public string InterectableInfoText;

    public EnvironmentInfo EnvironmentInfo;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.TryGetComponent(out _ownerOutline);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InterectableObjectRun()
    {
        if (transform.eulerAngles.y < 100)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0f,112f,0f));
            
        }
        
        else if (transform.eulerAngles.y > 100)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0f,0f,0f));

        }
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

}

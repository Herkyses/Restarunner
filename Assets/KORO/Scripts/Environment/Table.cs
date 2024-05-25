using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour,IInterectableObject
{
    [SerializeField] private string checkOrder = "Check Order";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InterectableObjectRun()
    {
        GameSceneCanvas.Instance.ShowOrderNoteBook();
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
        return checkOrder;
    }
}

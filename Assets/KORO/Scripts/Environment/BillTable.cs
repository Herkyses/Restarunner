using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillTable : MonoBehaviour,IInterectableObject
{
    
    public static BillTable Instance;

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
        CheckOrderBillsPanel.Instance.ActiveBillsPanel();
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
        return "Check Order Bills";
    }

    public void Move()
    {
        
    }
}

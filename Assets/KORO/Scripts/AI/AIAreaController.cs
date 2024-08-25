using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAreaController : MonoBehaviour
{
    public bool Interactabled;

    public AIController AIController;

    public WaiterController WaiterController;
    // Start is called before the first frame update

    private void Awake()
    {
        gameObject.TryGetComponent(out AIController);
        gameObject.TryGetComponent(out WaiterController);
    }

    
    

    
    

    public void InteractabelControl()
    {
        Interactabled = true;
    }
    public void InteractabelDeactive()
    {
        Interactabled = false;
    }
}

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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!Interactabled)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.2f);

            foreach (Collider col in colliders)
            {
                var interfacezort = col.gameObject.GetComponent<IAIInteractable>();
                if (interfacezort != null)
                {
                    interfacezort.StartState(transform);
                }
                else
                {
                
                }
            } 
        }*/
    }

    public void StartInteractableObject(Enums.AIStateType aiStateType)
    {
        if (!Interactabled)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.2f);

            foreach (Collider col in colliders)
            {
                if (aiStateType == Enums.AIStateType.Customer)
                {
                    var interfacezort = col.gameObject.GetComponent<IAIInteractable>();
                    if (interfacezort != null)
                    {
                        interfacezort.StartState(this,Enums.AIStateType.Customer);
                    }
                }
                else if (aiStateType == Enums.AIStateType.Waiter)
                {
                    var interfacezort = col.gameObject.GetComponent<IAIInteractable>();
                    if (interfacezort != null)
                    {
                        interfacezort.StartState(this,Enums.AIStateType.Waiter);
                    }
                }
                
            } 
        }
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

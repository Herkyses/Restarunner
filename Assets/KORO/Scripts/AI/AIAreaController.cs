using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAreaController : MonoBehaviour
{
    public bool Interactabled;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Interactabled)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAreaController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

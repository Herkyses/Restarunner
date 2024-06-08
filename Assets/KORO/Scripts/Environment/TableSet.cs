using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableSet : MonoBehaviour
{
    public Table table;
    public int groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        groundLayer = LayerMask.NameToLayer("Ground");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckGround()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == groundLayer)
            {
                table.IsTableSetTransform = true;
                return;
            }
        }

        table.IsTableSetTransform = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != groundLayer)
        {
            table.IsTableSetTransform = false;
        }
        else
        {
            Debug.Log("asdadasddas");

            table.IsTableSetTransform = true;

        }
    }
}

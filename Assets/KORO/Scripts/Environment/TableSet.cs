using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableSet : MonoBehaviour
{
    public Table table;
    public int groundLayer;
    public int tableIndexLayer;
    public int tableTypeID;
    private Outline _outline;

    // Start is called before the first frame update
    void Start()
    {
        groundLayer = LayerMask.NameToLayer("Ground");
        tableIndexLayer = LayerMask.NameToLayer("TableSet");
        //transform.position = MapManager.Instance.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckGround()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.8f);
        var checkControl = false;
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.gameObject.layer == tableIndexLayer)
            {
                return;
            }
           
        }
        foreach (Collider collider in colliders)
        {
            
            if (collider.gameObject != gameObject && collider.gameObject.layer == groundLayer) 
            {
                table.IsTableSetTransform = true;
                return;
            }
        }

       

        table.IsTableSetTransform = false;
    }
    /*private void OnTriggerEnter(Collider other)
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
    }*/
}

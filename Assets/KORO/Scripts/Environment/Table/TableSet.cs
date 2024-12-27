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
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private TableMovement tableMovement;

    // Start is called before the first frame update
    void Start()
    {
        groundLayer = LayerMask.NameToLayer("Ground");
        tableIndexLayer = LayerMask.NameToLayer("Decoration");
        //transform.position = MapManager.Instance.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckGround()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.8f, _layerMask);

        bool checkControl = false;

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == gameObject) 
                continue;

            int layer = collider.gameObject.layer;

            if (layer != groundLayer && layer == tableIndexLayer)
            {
                tableMovement.IsTableSetTransform = false;
                return;
            }

            if (layer == groundLayer && layer != tableIndexLayer)
            {
                checkControl = true;
            }
        }

        // Kontrol sonucunu belirle
        if (tableMovement.IsTableSetTransform != checkControl)
        {
            tableMovement.IsTableSetTransform = checkControl;
        }
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

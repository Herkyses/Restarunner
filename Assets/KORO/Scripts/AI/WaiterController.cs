using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterController : MonoBehaviour
{
    [SerializeField] private List<OrderDataStruct> _orderList ;
    public int TableNumber ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddOrder(List<OrderDataStruct> orderDataStructList)
    {
        _orderList = orderDataStructList;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrderController : MonoBehaviour
{
    public static PlayerOrderController Instance;
    
    public List<Orders> OrderList = new List<Orders>();
    public List<Food> OrderFoodList = new List<Food>();
    public Transform FoodTransform;
    
    
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
    
    public void SetOrder(int tableNumber, List<OrderDataStruct> orderDataStruct)
    {
        var order = new Orders()
        {
            TableNumber = tableNumber,
            OrderDataStructs = orderDataStruct
        };
        OrderList.Add(order);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrderController : MonoBehaviour
{
    public static PlayerOrderController Instance;
    
    public List<Orders> OrderList = new List<Orders>();
    public List<Food> OrderFoodList = new List<Food>();
    public Food Food;
    public TableBill TableBill;
    public Transform FoodTransform;
    public bool TakedFood;
    public bool TakedTableBill;
    
    
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

    public void TakeFood(Food food)
    {
        if (!TakedFood)
        {
            Food = food;
            TakedFood = true;
            OrderFoodList.Add(food);
            food.transform.position = PlayerOrderController.Instance.FoodTransform.position;
            food.transform.rotation = PlayerOrderController.Instance.FoodTransform.rotation;
            food.transform.SetParent(PlayerOrderController.Instance.FoodTransform);
        }
        
    }
    public void TakeBill(TableBill tableBill)
    {
        if (!TakedTableBill)
        {
            TableBill = tableBill;
            TakedTableBill = true;
            //OrderFoodList.Add(food);
            TableBill.transform.position = PlayerOrderController.Instance.FoodTransform.position;
            //TableBill.transform.rotation = PlayerOrderController.Instance.FoodTransform.rotation;
            TableBill.transform.SetParent(PlayerOrderController.Instance.FoodTransform);
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

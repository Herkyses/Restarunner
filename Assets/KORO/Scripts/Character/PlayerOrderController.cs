using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerOrderController : MonoBehaviour
{
    public static PlayerOrderController Instance;
    
    public List<Orders> OrderList = new List<Orders>();
    public List<Food> OrderFoodList = new List<Food>();
    public Food Food;
    public FoodTable FoodTable;
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

    public void TakeFood(FoodTable food)
    {
        FoodTable = food;
        if (!TakedFood)
        {
            Food = food.Food;
            TakedFood = true;
            OrderFoodList.Add(food.Food);

            food.transform.DOLocalMove(Vector3.zero, 0.2f);
            food.transform.DOLocalRotate(Vector3.zero, 0.2f);
            food.transform.SetParent(CameraController.Instance.PlayerTakedObjectTransformParent);
        }
        
    }
    public void TakeBill(TableBill tableBill)
    {
        if (!TakedTableBill)
        {
            TableBill = tableBill;
            TakedTableBill = true;
            //OrderFoodList.Add(food);
            TableBill.transform.SetParent(CameraController.Instance.PlayerTakedObjectTransformParent);
            TableBill.transform.localPosition = new Vector3(0.2f,0,0);
            TableBill.transform.localRotation = Quaternion.Euler(new Vector3(0f,0,30));
            /*TableBill.transform.position = FoodTransform.position;
            var zort = FoodTransform.transform.rotation.eulerAngles + new Vector3(0f, 0f, -90f);
            TableBill.transform.localRotation = Quaternion.Euler(zort);
            TableBill.transform.SetParent(FoodTransform);*/
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

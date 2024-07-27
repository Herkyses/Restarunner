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

    public void TakeFood(FoodTable food,bool isFoodFinished)
    {
        FoodTable = food;
        
        if (!TakedFood)
        {
            if (!isFoodFinished)
            {
                Food = food.Food;
                TakedFood = true;
                OrderFoodList.Add(food.Food);
                Player.Instance.PlayerStateType = Enums.PlayerStateType.GiveFood;
            }
            else
            {
                Player.Instance.PlayerStateType = Enums.PlayerStateType.Trays;
            }
            food.transform.DOLocalMove(Vector3.zero, 0.2f);
            food.transform.DOLocalRotate(Vector3.zero, 0.2f);
            food.transform.SetParent(CameraController.Instance.PlayerTakedObjectTransformParent);
            //Player.Instance.PlayerStateType = Enums.PlayerStateType.GiveFood;
            //Player.Instance.PlayerTakedObject = food.gameObject;
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
            tableBill.transform.DOLocalMove(new Vector3(0.2f, 0, 0), 0.2f);
            tableBill.transform.DOLocalRotate(new Vector3(0f,0,30), 0.2f);
            Player.Instance.PlayerStateType = Enums.PlayerStateType.OrderBill;

            //TableBill.transform.localRotation = Quaternion.Euler(new Vector3(0f,0,30));
            /*TableBill.transform.position = FoodTransform.position;
            var zort = FoodTransform.transform.rotation.eulerAngles + new Vector3(0f, 0f, -90f);
            TableBill.transform.localRotation = Quaternion.Euler(zort);
            TableBill.transform.SetParent(FoodTransform);*/
        }
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        SetOrderList();
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

    public void SetOrderList()
    {
        OrderList.Clear();
        for (int i = 0; i < TableController.Instance.TableSets.Count; i++)
        {
            var order = new Orders();
            order.OrderDataStructs = new List<OrderDataStruct>();
            order.TableNumber = TableController.Instance.TableSets[i].table.TableNumber;
            OrderList.Add(order);
        }
    }

    public void AddNewOrderList()
    {
        
    }
}

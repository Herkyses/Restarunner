using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class Table : MonoBehaviour,IInterectableObject, IAIInteractable
{
    [SerializeField] private string checkOrder = "Check Order";
    [SerializeField] private List<OrderDataStruct> _orderList ;
    public List<AIController> _aiControllerList ;
    public bool IsTableAvailable ;
    public bool IsTableMove ;
    public bool IsTableSetTransform ;
    public bool IsTableFoodFinished ;
    public int TableNumber ;
    public int TableCapacity;
    public int CustomerCount;
    public int groundLayer;
    public float TotalBills;
    public TextMeshProUGUI TableNumberText;
    public TableSet TableSet;

    public List<Chair> ChairList;
    public List<Transform> FoodTransformList;
    public Transform BillPanel;
    private void OnEnable()
    {
        Chair.GivedOrder += CreateOrdersWithAction;
    }

    private void OnDisable()
    {
        Chair.GivedOrder -= CreateOrdersWithAction;

    }

    private void Update()
    {
        if (IsTableMove)
        {
            MoveStart();
        }
        
    }

    public bool CheckAllCustomerFinishedFood()
    {
        for (int i = 0; i < _aiControllerList.Count; i++)
        {
            if (!_aiControllerList[i].IsFinishedFood)
            {
                return false;
            }
        }

        return true;
    }

    public void AllFoodfinished()
    {
        IsTableFoodFinished = true;
        BillPanel.gameObject.SetActive(true);
    }

    public void StartState(Transform AITransform)
    {
        AvailabilityControl();
        var chair = CheckChairAvailable();
        if (chair)
        {
            chair.isChairAvailable = false;
            AITransform.position = chair.transform.position;
            AITransform.rotation = chair.transform.rotation;
            var stateMAchineController = AITransform.gameObject.GetComponent<AIStateMachineController>();
            stateMAchineController.AIChangeState(stateMAchineController.AISitState);
            AITransform.gameObject.GetComponent<AIAreaController>().InteractabelControl();
            _aiControllerList.Add(AITransform.gameObject.GetComponent<AIController>());
            var orderIndex = Random.Range(0, 2);
            stateMAchineController.GetComponent<AIController>().AIOwnerTable = this;
            stateMAchineController.GetComponent<AIController>().AIOwnerChair = chair;
            SetOrderTable(stateMAchineController.GetComponent<AIController>(),orderIndex);
            if (OrderPanelController.Instance.OpenedTableNumber == TableNumber)
            {
                Chair.GivedOrder?.Invoke(TableNumber);
            }
        }
        
    }
    public void SetOrderTable(AIController aiController,int orderIndex)
    {
        
        var newOrder = new OrderDataStruct()
        {
            OrderType = (Enums.OrderType) orderIndex,
        };
        aiController.FoodDataStruct = newOrder;
        SetOrder(newOrder);
    }

    public void RemoveOrder(OrderDataStruct orderDataStruct)
    {
        _orderList.Remove(orderDataStruct);
    }
    public Chair CheckChairAvailable()
    {
        for (int i = 0; i < ChairList.Count; i++)
        {
            if (ChairList[i].isChairAvailable)
            {
                return ChairList[i];
            }
        }

        return null;
    }
    public void AvailabilityControl()
    {
        if (CustomerCount < TableCapacity)
        {
            CustomerCount++;
        }
        else
        {
            IsTableAvailable = false;
        }
    }

    private void Start()
    {
        groundLayer = LayerMask.NameToLayer("Ground");

        IsTableAvailable = false;
        TableNumberText.text = TableNumber.ToString();
    }

    public void CreateOrdersWithAction(int tableNumber)
    {
        if (tableNumber == TableNumber)
        {
            InterectableObjectRun();
        }
        
    }

    public void SetOrder(OrderDataStruct singleOrder)
    {
        _orderList.Add(singleOrder);
    }
    public void InterectableObjectRun()
    {
        if (!PlayerOrderController.Instance.TakedFood)
        {
            OrderPanelController.Instance.ShowOrder(_orderList,TableNumber);
            OpenOrderPanels();
        }
    }

    public void OpenOrderPanels()
    {
        for (int i = 0; i < _aiControllerList.Count; i++)
        {
            StartCoroutine(_aiControllerList[i].FoodIcon());
        }
    }

    public void ShowOutline(bool active)
    {
        
    }

    public Outline GetOutlineComponent()
    {
        return null;
    }

    public string GetInterectableText()
    {
        if (!PlayerOrderController.Instance.TakedFood)
        {
            //return "GivedFoods";
        }

        if (!IsTableFoodFinished)
        {
            return checkOrder;
        }
        else
        {
            return "CheckBills";
        }
    }

    public void Move()
    {
        if (!IsTableMove)
        {
            IsTableMove = true;
        }
        
    }
    public void MoveStart()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        
        if (Physics.Raycast(ray, out hit))
        {
            float xValue = hit.point.x;
            float zValue = hit.point.z;
            TableSet.GetComponent<BoxCollider>().enabled = true;
            TableSet.transform.position = new Vector3(xValue,TableSet.transform.position.y,zValue); // Objenin pozisyonunu fare ile tıklanan noktaya taşı
            TableController.Instance.EnableTableSetCollider(true);
        } 
        if (Input.GetMouseButton(0))
        {
            TableSet.CheckGround();
            if (IsTableSetTransform)
            {
                //if(colliders.Length )
                TableSet.GetComponent<BoxCollider>().enabled = false;
                TableController.Instance.EnableTableSetCollider(false);
                IsTableSetTransform = false;
                IsTableMove = false;
            }
            
        }
        
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != groundLayer)
        {
            IsTableSetTransform = false;
        }
        else
        {
            Debug.Log("asdadasddas");

            IsTableSetTransform = true;

        }
    }*/
}

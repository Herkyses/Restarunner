using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class Table : MonoBehaviour,IInterectableObject, IAIInteractable
{
    [SerializeField] private string[] texts = new [] {"Check Order "};
    [SerializeField] private string[] textsForTable = new [] {"Drop","Rotate"};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    [SerializeField] private string[] textsButtonsForTable = new [] {"M0","R"};
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
    public int TableQuality;
    public float TotalBills;
    public TextMeshProUGUI TableNumberText;
    public TableSet TableSet;

    public List<Chair> ChairList;
    public List<Transform> FoodTransformList;
    public Transform BillPanel;

    private Outline _outline;
    private Player _player;
    private TableController tableController;
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
    

    public List<OrderDataStruct> GetOrders()
    {
        return _orderList;
    }

    public bool CheckAllCustomerFinishedFood()
    {
        if (_aiControllerList.Count > 0)
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
        return false;
    }

    public void AllFoodfinished()
    {
        IsTableFoodFinished = true;
        BillPanel.gameObject.SetActive(true);
    }

    public void ResetTable()
    {
        CheckOrderBillsPanel.Instance.UpdateBillList(TableNumber);
        TotalBills = 0;
        _aiControllerList.Clear();
        IsTableFoodFinished = false;
        BillPanel.gameObject.SetActive(false);
    }

    public void StartState(AIAreaController AIArea, Enums.AIStateType aiStateType)
    {
        switch (aiStateType)
        {
            case Enums.AIStateType.Customer:
                HandleCustomerState(AIArea);
                break;
            case Enums.AIStateType.Waiter:
                HandleWaiterState(AIArea);
                break;
        }
        
        
    }

    private void HandleCustomerState(AIAreaController aiArea)
    {
        AvailabilityControl();
        var chair = CheckChairAvailable();
        if (chair == null) return;

        AssignAIToChair(aiArea, chair);
        SetAIOrder(aiArea);
    
        TableController.GivedOrderForAIWaiter?.Invoke(this);
        if (OrderPanelController.Instance.OpenedTableNumber == TableNumber)
        {
            Chair.GivedOrder?.Invoke(TableNumber);
        }
    }
    private void HandleWaiterState(AIAreaController aiArea)
    {
        aiArea.WaiterController.AddOrder(_orderList);
    }
    
    private void AssignAIToChair(AIAreaController aiArea, Chair chair)
    {
        chair.isChairAvailable = false;
        aiArea.transform.position = chair.transform.position;
        aiArea.transform.rotation = chair.transform.rotation;

        var stateMachineController = aiArea.GetComponent<AIStateMachineController>();
        stateMachineController.AIChangeState(stateMachineController.AISitState);

        aiArea.InteractabelControl();
        _aiControllerList.Add(aiArea.GetComponent<AIController>());
        
        aiArea.AIController.SetTableInfo(this, chair);
    }
    private void SetAIOrder(AIAreaController aiArea)
    {
        var orderIndex = Random.Range(0, GameDataManager.Instance.OpenFoodDatas.Count);
        var orderType = GameDataManager.Instance.OpenFoodDatas[orderIndex].OrderType;

        SetOrderTable(aiArea.AIController, orderType);
    }
    public void SetOrderTable(AIController aiController,Enums.OrderType orderType)
    {
        
        var newOrder = new OrderDataStruct()
        {
            OrderType = orderType,
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
        texts = new []{"Check Order","Move"};
        textsButtons = new []{"E","H"};
        textsForTable = new [] {"Set up","Rotate"};
        textsButtonsForTable = new [] {"M0","R"};
        IsTableAvailable = true;
        TableNumberText.text = (TableNumber+1).ToString();
        _outline = GetComponent<Outline>();
        _player = Player.Instance;
        tableController = TableController.Instance;
    }

    public void TableInitialize()
    {
        IsTableAvailable = false;
        TableNumberText.text = (TableNumber+1).ToString();
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
        if (!_player.PlayerOrdersController.TakedFood && !_player.PlayerOrdersController.TakedTableBill)
        {
            if (CustomerCount > 0)
            {
                OrderPanelController.Instance.ShowOrder(_orderList,TableNumber);
                OpenOrderPanels();
            }
            
        }

        if (_player.PlayerOrdersController.TakedTableBill && _player.PlayerOrdersController.TableBill.OwnerTable == this)
        {
            if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 4)
            {
                PlayerPrefsManager.Instance.SavePlayerPlayerTutorialStep(10);
                TutorialManager.Instance.SetTutorialInfo(PlayerPrefsManager.Instance.LoadPlayerTutorialStep());
            }
            for (int i = 0; i < _aiControllerList.Count; i++)
            {
                _aiControllerList[i].AIStateMachineController.SetMoveStateFromOrderBill();
            }

            _player.FreePlayerStart();

            BillTable.Instance.UpdateTableBill(_player.PlayerOrdersController.TableBill);
            
            ResetTable();
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
        _outline.enabled = active;
    }

    public Outline GetOutlineComponent()
    {
        return _outline;
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
        if (!IsTableMove && !PlaceController.RestaurantIsOpen && CustomerCount == 0)
        {
            IsTableMove = true;
            Player.Instance.MoveObject(gameObject,Enums.PlayerStateType.MoveTable);
            GameSceneCanvas.Instance.ShowAreaInfoForTexts(textsForTable);
            GameSceneCanvas.Instance.ShowAreaInfoForTextsButtons(textsButtonsForTable);
            TableSet.GetComponent<BoxCollider>().enabled = false;
            TableController.Instance.EnableTableSetCollider(true);

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
            //TableSet.GetComponent<BoxCollider>().enabled = true;
            TableSet.transform.position = new Vector3(xValue,0,zValue); // Objenin pozisyonunu fare ile tıklanan noktaya taşı
            //TableController.Instance.EnableTableSetCollider(true);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            var tableRotat = TableSet.transform.rotation;
            var tableRotatTemp = Quaternion.Euler(new Vector3(tableRotat.eulerAngles.x,tableRotat.eulerAngles.y+90f,tableRotat.eulerAngles.z));

            TableSet.transform.rotation = tableRotatTemp;
        }
        if (Input.GetMouseButton(0))
        {
            TableSet.CheckGround();
            if (IsTableSetTransform)
            {
                MapManager.Instance.SaveMap();
                TableControl();
                Player.Instance.ActivatedRaycast(true);

                Player.Instance.TakedObjectNull();

                //if(colliders.Length )
                TableSet.GetComponent<BoxCollider>().enabled = false;
                tableController.EnableTableSetCollider(false);
                IsTableSetTransform = false;
                IsTableMove = false;
                if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 2)
                {
                    PlayerPrefsManager.Instance.SavePlayerPlayerTutorialStep(4);
                    TutorialManager.Instance.Initiliaze();
                }
            }
            
        }
        
    }
    public void TableControl()
    {
        var isTableHere = false;
        for (int i = 0; i < tableController.TableSets.Count; i++)
        {
            if (TableSet == tableController.TableSets[i])
            {
                return;
            }
        }
        TableSet.transform.SetParent(tableController.TableTransform);
        tableController.TableSetCapacity++;
        tableController.TableSets.Add(TableSet);
        tableController.UpdateTables();
        TableAvailablePanel.Instance.AddNewTable(this);
        CheckOrderBillsPanel.Instance.UpdatePanel(TableNumber,TotalBills);
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
    public void Open()
    {
        
    }
    public string[] GetInterectableTexts()
    {
        return texts;
    }
    public string[] GetInterectableButtons()
    {
        return textsButtons;
    }
    public Enums.PlayerStateType GetStateType()
    {
        if (IsTableFoodFinished)
        {
            return Enums.PlayerStateType.OrderBill;

        }
        else
        {
            return Enums.PlayerStateType.Free;

        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;


public class Table : MonoBehaviour,IInterectableObject, IAIInteractable
{
    [SerializeField] private string[] texts = new [] {"Check Order "};
    [SerializeField] private string[] textsForTable = new [] {"Set up","Rotate"};
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
    
    public float TableQuality = 5;
    public float TotalBills;
    public TextMeshProUGUI TableNumberText;
    public TableSet TableSet;

    public List<Chair> ChairList;
    public List<Transform> FoodTransformList;
    public Transform BillPanel;

    private Outline _outline;
    private Player _player;
    private TableController tableController;
    
    private GameSceneCanvas _gameSceneCanvas;
    


    private void OnEnable()
    {
        Chair.GivedOrder += CreateOrdersWithAction;
    }

    private void OnDisable()
    {
        Chair.GivedOrder -= CreateOrdersWithAction;

    }
    
    private void Start()
    {
        InitializeTable();

    }
    private void Update()
    {
        if (IsTableMove)
        {
            SetTablePosition();
        }
        
    }
    private void InitializeTable()
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
        tableController = ControllerManager.Instance.Tablecontroller;
        TableQuality = 5f;
        _gameSceneCanvas = GameSceneCanvas.Instance;
    }
    
    

    public List<OrderDataStruct> GetOrders()
    {
        return _orderList;
    }
    
    public bool CheckAllCustomerFinishedFood() =>
        _aiControllerList.Count > 0 && _aiControllerList.TrueForAll(ai => ai.IsFinishedFood);

    public void AllFoodfinished()
    {
        IsTableFoodFinished = true;
        BillPanel.gameObject.SetActive(true);
    }

    public void ResetTable()
    {
        ControllerManager.Instance._checkOrderBillsPanel.UpdateBillList(TableNumber);
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
        
        if (!IsChairAvailable()) return;
        AssignAIToChairAndOrder(aiArea);
        NotifyOrderUpdates();
    }
    private bool IsChairAvailable()
    {
        AvailabilityControl();
        return CheckChairAvailable() != null;
    }
    private void AssignAIToChairAndOrder(AIAreaController aiArea)
    {
        var chair = CheckChairAvailable();
        if (chair == null) return;

        AssignAIToChair(aiArea, chair);
        SetAIOrder(aiArea);
    }
    
    private void NotifyOrderUpdates()
    {
        TableController.GivedOrderForAIWaiter?.Invoke(this);

        if (ControllerManager.Instance._orderPanelController.OpenedTableNumber == TableNumber)
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
        HandleOrder();
        HandlePayment();
    }
    private void HandleOrder()
    {
        if (!_player.PlayerOrdersController.TakedFood && !_player.PlayerOrdersController.TakedTableBill)
        {
            if (CustomerCount > 0)
            {
                ControllerManager.Instance._orderPanelController.ShowOrder(_orderList, TableNumber);
                OpenOrderPanels();
            }
        }
    }
    private void HandlePayment()
    {
        if (_player.PlayerOrdersController.TakedTableBill && _player.PlayerOrdersController.TableBill.OwnerTable == this)
        {
            GameManager.Instance.CheckAndProgressTutorialStep(4, 100);

            foreach (var aiController in _aiControllerList)
            {
                aiController.AIStateMachineController.SetMoveStateFromOrderBill();
            }

            GameManager.Instance.ProcessOrderPayment(_player.PlayerOrdersController.TableBill.BillValue);

            _player.FreePlayerStart();
            BillTable.Instance.UpdateTableBill(_player.PlayerOrdersController.TableBill);
            ResetTable();
        }
    }
    
    public void OpenOrderPanels()
    {
        foreach (var aiController in _aiControllerList)
            StartCoroutine(aiController.FoodIcon());
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
        if (!IsTableMove && !ControllerManager.Instance.PlaceController.RestaurantIsOpen && CustomerCount == 0)
        {
            InitiateTableMovement();
        }
        SetTablePosition();
        
    }
    private void InitiateTableMovement()
    {
        IsTableMove = true;
        _gameSceneCanvas = _gameSceneCanvas ?? GameSceneCanvas.Instance;
        _gameSceneCanvas.MoveObjectInfo(textsForTable, textsButtonsForTable, Enums.PlayerStateType.MoveTable);
        TableSet.GetComponent<BoxCollider>().enabled = false;
        ControllerManager.Instance.Tablecontroller.EnableTableSetCollider(true);
    }
    public void SetTablePosition()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out var hit))
        {
            TableSet.transform.position = new Vector3(hit.point.x, 0.14f, hit.point.z);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            var tableRotat = TableSet.transform.rotation;
            var tableRotatTemp = Quaternion.Euler(new Vector3(tableRotat.eulerAngles.x,tableRotat.eulerAngles.y+90f,tableRotat.eulerAngles.z));

            TableSet.transform.rotation = tableRotatTemp;
        }
        PlaceTable();
    }
    public void PlaceTable()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TableSet.CheckGround();
            if (IsTableSetTransform)
            {
                FinalizeTablePlacement();
            }
        }
    }
    private void FinalizeTablePlacement()
    {
        UpdateTableStatus();
        ResetPlayerState();
        tableController.EnableTableSetCollider(false);
        IsTableSetTransform = false;
        IsTableMove = false;
        GameManager.Instance.HandleTablePlacementCompletion();
        HandleTutorialProgression();
    }
    private void UpdateTableStatus()
    {
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
        ControllerManager.Instance.TableAvailablePanel.AddNewTable(this);
        Debug.Log("tablenumber:" + TableNumber);

        ControllerManager.Instance._checkOrderBillsPanel.UpdatePanel(TableNumber, TotalBills);
    }

    private void ResetPlayerState()
    {
        _player.ActivatedRaycast(true);
        _player.TakedObjectNull();
    }
    private void HandleTutorialProgression()
    {
        if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 2)
        {
            PlayerPrefsManager.Instance.SavePlayerPlayerTutorialStep(4);
            TutorialManager.Instance.Initiliaze();
        }
    }
    
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

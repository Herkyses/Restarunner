using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;


public class Table : MonoBehaviour,IInterectableObject, IAIInteractable
{
    
    [SerializeField] private TableSetData tableSetData;
    //[SerializeField] private List<OrderDataStruct> _orderList ;
    [SerializeField] private CustomerStateManager _customerStateManager;
    [SerializeField] private OrderHandler _orderHandler;
    
   // public List<AIController> _aiControllerList ;
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
    [Inject] private TableController tableController;
    [Inject] private PlayerPrefsManager playerPrefsManager;
    
    private GameSceneCanvas _gameSceneCanvas;
    [SerializeField] private Material _material;
    private Material _currentMaterial;
    private Renderer _renderer;


    public CustomerStateManager GetCustomerStateManager()
    {
        return _customerStateManager;
    }
    public OrderHandler GetOrderHandler()
    {
        return _orderHandler;
    }
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
        //InitializeTable();
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
        _renderer = GetComponent<Renderer>();
        //_material = _renderer.sharedMaterial;
        groundLayer = LayerMask.NameToLayer("Ground");
        IsTableAvailable = true;
        TableNumberText.text = (TableNumber+1).ToString();
        _outline = GetComponent<Outline>();
        _player = Player.Instance;
        tableSetData = tableController.GetTableSetData();
        TableQuality = 5f;
        _gameSceneCanvas = GameSceneCanvas.Instance;
        playerPrefsManager = PlayerPrefsManager.Instance;
    }
    
    

    /*public List<OrderDataStruct> GetOrders()
    {
        return _orderList;
    }*/

    public void AllFoodfinished()
    {
        IsTableFoodFinished = true;
        BillPanel.gameObject.SetActive(true);
    }

    public void ResetTable()
    {
        ControllerManager.Instance._checkOrderBillsPanel.UpdateBillList(TableNumber);
        TotalBills = 0;
        _customerStateManager._aiControllerList.Clear();
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
        
        _customerStateManager.StartState(aiArea,this);
        
        //AssignAIToChairAndOrder(aiArea);
        //NotifyOrderUpdates();
    }
    private bool IsChairAvailable()
    {
        AvailabilityControl();
        return _customerStateManager.CheckChairAvailable() != null;
    }
    
    
    
    private void HandleWaiterState(AIAreaController aiArea)
    {
        aiArea.WaiterController.AddOrder(_orderHandler.GetOrders());
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
        InitializeTable();
    }

    public void CreateOrdersWithAction(int tableNumber)
    {
        if (tableNumber == TableNumber)
        {
            InterectableObjectRun();
        }
        
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
                ControllerManager.Instance._orderPanelController.ShowOrder(_orderHandler.GetOrders(), TableNumber);
                OpenOrderPanels();
            }
        }
    }
    private void HandlePayment()
    {
        if (_player.PlayerOrdersController.TakedTableBill && _player.PlayerOrdersController.TableBill.OwnerTable == this)
        {
            ControllerManager.Instance._checkOrderBillsPanel.ActivePayedPanel(this);
            //AIPayed();
        }
    }

    public void AIPayed()
    {
        GameManager.Instance.CheckAndProgressTutorialStep(5, 100);

        foreach (var aiController in _customerStateManager._aiControllerList)
        {
            aiController.AIStateMachineController.SetMoveStateFromOrderBill();
        }

        GameManager.Instance.ProcessOrderPayment(_player.PlayerOrdersController.TableBill.BillValue);

        _player.FreePlayerStart();
        BillTable.Instance.UpdateTableBill(_player.PlayerOrdersController.TableBill);
        ResetTable();
    }
    
    
    public void OpenOrderPanels()
    {
        foreach (var aiController in _customerStateManager._aiControllerList)
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
            return tableSetData.checkOrder;
        }
        else
        {
            return "CheckBills";
        }
    }

    
    
    public void Move()
    {
        if (!IsTableMove && !ControllerManager.Instance.PlaceController.IsRestaurantOpen && CustomerCount == 0)
        {
            Debug.Log("zbombomove");

            InitiateTableMovement();

        }
        SetTablePosition();
        
    }
    private void InitiateTableMovement()
    {
        IsTableMove = true;
        _gameSceneCanvas = _gameSceneCanvas ?? GameSceneCanvas.Instance;
        if (tableSetData == null)
        {
            InitializeTable();

        }
        _gameSceneCanvas.MoveObjectInfo(tableSetData.textsForTable, tableSetData.textsButtonsForTable, Enums.PlayerStateType.MoveTable);
        TableSet.GetComponent<BoxCollider>().enabled = false;
        ControllerManager.Instance.Tablecontroller.EnableTableSetCollider(true);
        GetComponent<BoxCollider>().isTrigger = true;
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
        // Mouse tıklamasını kontrol edin
        TableSet.CheckGround();

        if (Input.GetMouseButtonDown(0))
        {
            if (IsTableSetTransform)
            {
                FinalizeTablePlacement();
            }
            return;
        }

        Material newMaterial = IsTableSetTransform
            ? tableController.GetSetableMaterial()
            : tableController.GetWrongMaterial();

        if (_currentMaterial != newMaterial)
        {
            _renderer.sharedMaterial = newMaterial;
            _currentMaterial = newMaterial;
            SetChairMaterial(newMaterial);
        }
    }

    public void SetChairMaterial(Material material)
    {
        for (int i = 0; i < ChairList.Count; i++)
        {
            ChairList[i].GetComponent<Renderer>().sharedMaterial = material;
        }
    }
    private void FinalizeTablePlacement()
    {
        UpdateTableStatus();
        ResetPlayerState();
        tableController.EnableTableSetCollider(false);
        IsTableSetTransform = false;
        IsTableMove = false;
        GetComponent<BoxCollider>().isTrigger = false;
        GetComponent<Renderer>().sharedMaterial = _material;
        SetChairMaterial(_material);
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
        if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 3)
        {
            PlayerPrefsManager.Instance.SavePlayerTutorialStep(5);
            TutorialManager.Instance.Initiliaze();
        }
    }
    
    public void Open()
    {
        
    }
    public string[] GetInterectableTexts()
    {
        return tableSetData.texts;
    }
    public string[] GetInterectableButtons()
    {
        return tableSetData.textsButtons;
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

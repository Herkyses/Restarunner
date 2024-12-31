using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;


public class Table : MonoBehaviour,IInterectableObject, IAIInteractable,IMovable
{
    
    [SerializeField] private TableSetData tableSetData;
    //[SerializeField] private List<OrderDataStruct> _orderList ;
    [SerializeField] private CustomerStateManager _customerStateManager;
    [SerializeField] private OrderHandler _orderHandler;
    [SerializeField] private TableMovement _tableMovement;
    
   // public List<AIController> _aiControllerList ;
    public bool IsTableAvailable ;
    public bool IsTableFoodFinished ;
    public int TableNumber ;
    public int TableCapacity;
    public int CustomerCount;
    
    public float TableQuality = 5;
    public float TotalBills;
    public TextMeshProUGUI TableNumberText;
    public TableSet TableSet;

    public List<Transform> FoodTransformList;
    public Transform BillPanel;

    private Outline _outline;
    private Player _player;
    [Inject] private TableController tableController;
    [Inject] private PlayerPrefsManager playerPrefsManager;
    
    [SerializeField] private Renderer[] _rendererList;


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
        _tableMovement.Initiliaze(ControllerManager.Instance.Tablecontroller);
    }
    
    public void InitializeTable()
    {
        IsTableAvailable = true;
        TableNumberText.text = (TableNumber+1).ToString();
        _outline = GetComponent<Outline>();
        _player = Player.Instance;
        tableController = ControllerManager.Instance.Tablecontroller;
        tableSetData = tableController.GetTableSetData();
        TableQuality = 5f;
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
       
    }
   
   
    
    public void FinalizeTablePlacement()
    {
        ResetPlayerState();
        UpdateTableStatus();
        //GameManager.Instance.HandleTablePlacementCompletion();
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
        if (playerPrefsManager.LoadPlayerTutorialStep() == 3)
        {
            playerPrefsManager.SavePlayerTutorialStep(5);
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

    public void Movement()
    {
        _tableMovement.InitiateTableMovement();
    }

    public void PlacedObject()
    {
        _tableMovement.FinalizeTableMovement();
    }

    public Transform GetMoveableObjectTransform()
    {
        return TableSet.transform;
    }

    public Renderer[] GetRenderers()
    {
        return _rendererList;
    }
}

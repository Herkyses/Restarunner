using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class AIStateMachineController : MonoBehaviour
{
    public AIBaseState CurrentState;
    public AIClapState AIClapState;
    public AIIdleState AIIdleState;
    public AIMoveState AIMoveState;
    public AISitState AISitState;
    public AITargetSitState AITargetSitState;
    public AITargetRestaurantState AITargetRestaurantState;
    public AIWaitPlayerState AIWaitPlayerState;
    public AIChefState AIChefState;
    public AIEatState AIEatState;
    public AIRunFromRestaurantState AIRunFromRestaurantState;

    public bool IsFriend;
    
    
    public AIWaiterState AIWaiterState;
    public AIWaiterMoveState AIWaiterMoveState;
    public AIWaiterMoveChefState AIWaiterMoveChefState;
    public AIWaiterGiveOrderState AIWaiterGiveOrderState;
    public AIWaiterTakeOrderBillState AIWaiterTakeOrderBillState;
    public AIWaiterGiveOrderBillState AIWaiterGiveOrderBillState;
    public AIWaiterTakeFoodState AIWaiterTakeFoodState;
    public AIWaiterGiveFoodState AIWaiterGiveFoodState;
    
    
    public AIController AIController;
    public AIAnimationController AIAnimationController;
    public AIWaitTimeController AIWaitTimeController;
    public WaiterController AIWaiterController;
    public AIAreaController AIAreaController;
    public Transform AITargetSitTransform;
    public Transform AITargetTableTransform;

    public List<AIController> Friends;

    public Table OwnerTable;
    //public static Action<float> PayedOrderBill; 
    public static Action CreateRubbish; 
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private bool _isAIChef;
    [SerializeField] private bool _isAIWaiter;



    

    

    // Update is called once per frame
    void Update()
    {
        CurrentState?.UpdateState();
    }

    public void Initialize(bool isFriend = false)
    {
        gameObject.TryGetComponent(out AIController);
        gameObject.TryGetComponent(out AIWaitTimeController);
        gameObject.TryGetComponent(out AIAreaController);
        gameObject.TryGetComponent(out AIWaiterController);
        
        _playerAnimator = gameObject.GetComponent<Animator>();
        AIAnimationController = gameObject.GetComponent<AIAnimationController>();
        AIIdleState = new AIIdleState(this);
        AIClapState = new AIClapState(this);
        AIMoveState = new AIMoveState(this);
        AISitState = new AISitState(this);
        AIEatState = new AIEatState(this);
        AITargetSitState = new AITargetSitState(this);
        AITargetRestaurantState = new AITargetRestaurantState(this);
        AIWaitPlayerState = new AIWaitPlayerState(this);
        AIChefState = new AIChefState(this);
        AIWaiterState = new AIWaiterState(this);
        AIWaiterMoveState = new AIWaiterMoveState(this);
        AIWaiterMoveChefState = new AIWaiterMoveChefState(this);
        AIWaiterGiveOrderState = new AIWaiterGiveOrderState(this);
        AIWaiterTakeOrderBillState = new AIWaiterTakeOrderBillState(this);
        AIWaiterGiveOrderBillState = new AIWaiterGiveOrderBillState(this);
        AIWaiterTakeFoodState = new AIWaiterTakeFoodState(this);
        AIWaiterGiveFoodState = new AIWaiterGiveFoodState(this);
        AIRunFromRestaurantState = new AIRunFromRestaurantState(this);
        IsFriend = isFriend;
        if (!IsFriend) AIInitialize();
    }
    
    public void AIInitialize()
    {
        if (_isAIChef)
        {
            AIChangeState(AIChefState);
        }
        else if (_isAIWaiter)
        {
            AIChangeState(AIWaiterState);
        }
        else
        {
            AIBaseState initialState = ControllerManager.Instance.PlaceController.RestaurantIsOpen
                ? (Random.value < 0.5f ? (AITargetRestaurantState) : (AIMoveState))
                : (AIMoveState);
            
            AIChangeState(initialState);
        }
    }

    public void AIChangeState(AIBaseState currentState)
    {

        if (CurrentState != null)
        {
            CurrentState.ExitState();
            CurrentState = null; 
        }

        CurrentState = currentState;
        CurrentState.EnterState();

    }
    
    
    // TargetSitState//
    public void TargetSitStateStart()
    {
        
    }
    // TargetSitState//
    public void StartEatState()
    {
        StartCoroutine(EatCoroutine());
    }

    public IEnumerator EatCoroutine()
    {
        yield return new WaitForSeconds(5f);
        AIController.DeactivatedFoodIcon();
        
        UpdateOrderBill();    
        ProcessFoodCompletion();
    }
    private void UpdateOrderBill()
    {
        var orderBill = GameDataManager.Instance.GetOrderBill(AIController.FoodDataStruct.OrderType);
        AIController.AIOwnerTable.TotalBills += orderBill;
        ControllerManager.Instance._checkOrderBillsPanel.UpdatePanel(AIController.AIOwnerTable.TableNumber, AIController.AIOwnerTable.TotalBills);
    }
    private void ProcessFoodCompletion()
    {
        AIController.AIOwnerFood.EatedFood();
        AIController.AIOwnerFood.IsFoodFinished = true;
        AIController.AIOwnerFood.IsFoodServiced = false;
        AIController.IsFinishedFood = true;
        AIController.AIOwnerTable.RemoveOrder(AIController.FoodDataStruct);
        //RubbishManager.Instance.CreateRubbishFromAI();
        CreateRubbish?.Invoke();
        if (AIController.AIOwnerTable.CheckAllCustomerFinishedFood())
        {
            HandleTableCompletion();
        }
    }

    private void HandleTableCompletion()
    {
        AIController.AIOwnerTable.AllFoodfinished();
        
        if (Random.value < 0.5f && PlayerPrefsManager.Instance.LoadPlayerTutorialStep() != 5)
        {
            AIController.AIOwnerTable.ResetTable();
            AIController.IsBadGuy = true;
            ResetAI();
            AIChangeState(AIRunFromRestaurantState);
            SetFriendsStateForRunFromRestaurant(AIRunFromRestaurantState);
        }
    }
    private void SetFriendsStateForRunFromRestaurant(AIBaseState state)
    {
        foreach (var friend in Friends)
        {
            friend.AIStateMachineController.AIChangeState(state);
            friend.AIStateMachineController.ResetAI();
        }
    }
    public void SetFriendsState()
    {
        Friends.ForEach(friend =>
        {
            friend.AIStateMachineController.AITargetSitTransform = OwnerTable.transform;
            friend.AIStateMachineController.OwnerTable = OwnerTable;
            friend.AIStateMachineController.AIChangeState(friend.AIStateMachineController.AITargetSitState);
            friend._agent.destination = OwnerTable.transform.position;
            friend._agent.speed = 1f;
        });
    }

    public void SetMoveStateFromOrderBill()
    {
        //AIController.AIOwnerTable._aiControllerList.Remove(AIController);
        GameSceneCanvas.Instance.AddPopularity();
        AIController.CalculatePopularityRate();

        //GameManager.PayedOrderBill?.Invoke(GameDataManager.Instance.GetFoodPrice(AIController.FoodDataStruct.OrderType));
        ResetAI();
        AIChangeState(AIMoveState);
        
    }
    public void SetMoveStateForRunState()
    {
        //AIController.AIOwnerTable._aiControllerList.Remove(AIController);
        ResetAI();
    }

    public void ResetAI()
    {
        ControllerManager.Instance.TableAvailablePanel.RedAvailability(AIController.AIOwnerTable.TableNumber);
        ControllerManager.Instance.TableAvailablePanel.CheckTable(AIController.AIOwnerTable.TableNumber);
       
        AIController.AIOwnerChair.isChairAvailable = true;
        AIController.IsSitting = false;
        AIController.AIOwnerTable.CustomerCount--;
        if (!AIController.AIOwnerTable.IsTableAvailable)
        {
            AIController.AIOwnerTable.IsTableAvailable = true;
        }
        AIAreaController.InteractabelDeactive();
        AIController.IsFinishedFood = false;
        AIController.IsTakedFood = false;
        OwnerTable = null;
    }
}

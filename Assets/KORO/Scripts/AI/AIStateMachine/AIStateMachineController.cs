using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private bool _isAIChef;
    [SerializeField] private bool _isAIWaiter;


    

    

    // Update is called once per frame
    void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.UpdateState();
        }
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
        AIInitialize();

    }

    public void AIInitialize()
    {
        if (IsFriend)
        {
            return;
        }
        if (!_isAIChef && !_isAIWaiter)
        {
            if (PlaceController.RestaurantIsOpen)
            {
                //AIChangeState(AITargetRestaurantState);

                if (Random.value < 0.5f)
                {
                    AIChangeState(AITargetRestaurantState);
                }
                else
                {
                    AIChangeState(AIMoveState);
                }
            }
            else
            {
                AIChangeState(AIMoveState);
            }

        }
        else if(_isAIChef)
        {
            AIChangeState(AIChefState);
        }
        else if(_isAIWaiter)
        {
            AIChangeState(AIWaiterState);
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
    public void EatStateStart()
    {
        StartCoroutine(EatDuring());
    }

    public IEnumerator EatDuring()
    {
        yield return new WaitForSeconds(5f);
        AIController.DeactivatedFoodIcon();
        
        AIController.AIOwnerTable.TotalBills += GameDataManager.Instance.GetOrderBill(AIController.FoodDataStruct.OrderType);
        AIController.IsFinishedFood = true;
        CheckOrderBillsPanel.Instance.UpdatePanel(AIController.AIOwnerTable.TableNumber,AIController.AIOwnerTable.TotalBills);    
        //Destroy(AIController.AIOwnerFood.Food.gameObject);
        AIController.AIOwnerFood.EatedFood();
        AIController.AIOwnerFood.IsFoodFinished = true;
        AIController.AIOwnerTable.RemoveOrder(AIController.FoodDataStruct);
        if (AIController.AIOwnerTable.CheckAllCustomerFinishedFood())
        {
            AIController.AIOwnerTable.AllFoodfinished();
            //if (Random.value < 0.5f)
            //{
                AIController.AIOwnerTable.ResetTable();
                AIChangeState(AIRunFromRestaurantState);
                AIController.IsBadGuy = true;
                ResetAI();
                if (Friends.Count > 0)
                {
                    for (int i = 0; i < Friends.Count; i++)
                    {
                        Friends[i].AIStateMachineController.AIChangeState(Friends[i].AIStateMachineController.AIRunFromRestaurantState);
                        Friends[i].AIStateMachineController.ResetAI();
                    }
                }
            //}
            
        }
        //AIController.AIOwnerChair.isChairAvailable = true;
        //AIController.AIOwnerTable.CustomerCount--;
        //AIChangeState(AIMoveState);
    }

    public void SetFriendsState()
    {
        if (Friends.Count > 0)
        {
            for (int i = 0; i < Friends.Count; i++)
            {
                Friends[i].AIStateMachineController.AITargetSitTransform = OwnerTable.transform;
                Friends[i].AIStateMachineController.OwnerTable = OwnerTable;
                Friends[i].AIStateMachineController.AIChangeState(Friends[i].AIStateMachineController.AITargetSitState);
                Friends[i]._agent.destination = OwnerTable.transform.position;
                Friends[i]._agent.speed = 1f;
                //TableAvailablePanel.Instance.RemoveFromCustomerList(Friends[i].AgentID);
            }
        }
    }

    public void SetMoveStateFromOrderBill()
    {
        //AIController.AIOwnerTable._aiControllerList.Remove(AIController);
        
        GameSceneCanvas.Instance.AddPopularity();
        GameManager.PayedOrderBill?.Invoke(GameDataManager.Instance.GetFoodPrice(AIController.FoodDataStruct.OrderType));
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
        TableAvailablePanel.Instance.RedAvailability(AIController.AIOwnerTable.TableNumber);
        TableAvailablePanel.Instance.CheckTable(AIController.AIOwnerTable.TableNumber);
       
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

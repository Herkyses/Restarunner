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
    public AIWaiterState AIWaiterState;
    public AIController AIController;
    public AIAnimationController AIAnimationController;
    public AIWaitTimeController AIWaitTimeController;
    public AIAreaController AIAreaController;
    public Transform AITargetSitTransform;
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

    void Start()
    {
        gameObject.TryGetComponent(out AIController);
        gameObject.TryGetComponent(out AIWaitTimeController);
        gameObject.TryGetComponent(out AIAreaController);
        
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
        AIInitialize();

    }

    public void AIInitialize()
    {
        if (!_isAIChef)
        {
            if (PlaceController.RestaurantIsOpen)
            {
                var zort = Random.Range(0, 2);
                if (zort == 0)
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
        Destroy(AIController.AIOwnerFood.Food.gameObject);
        AIController.AIOwnerTable.RemoveOrder(AIController.FoodDataStruct);
        if (AIController.AIOwnerTable.CheckAllCustomerFinishedFood())
        {
            AIController.AIOwnerTable.AllFoodfinished();
        }
        //AIController.AIOwnerChair.isChairAvailable = true;
        //AIController.AIOwnerTable.CustomerCount--;
        //AIChangeState(AIMoveState);
    }

    public void SetMoveStateFromOrderBill()
    {
        //AIController.AIOwnerTable._aiControllerList.Remove(AIController);
        TableAvailablePanel.Instance.RedAvailability(AIController.AIOwnerTable.TableNumber);
        TableAvailablePanel.Instance.CheckTable(AIController.AIOwnerTable.TableNumber);
        GameSceneCanvas.Instance.AddPopularity();
        GameManager.PayedOrderBill?.Invoke(GameDataManager.Instance.GetFoodPrice(AIController.FoodDataStruct.OrderType));
        AIController.AIOwnerChair.isChairAvailable = true;
        AIController.IsSitting = false;
        AIController.AIOwnerTable.CustomerCount--;
        AIAreaController.InteractabelDeactive();
        AIController.IsFinishedFood = false;
        AIChangeState(AIMoveState);
    }
}

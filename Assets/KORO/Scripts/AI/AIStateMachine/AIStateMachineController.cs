using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public AIController AIController;
    public AIAnimationController AIAnimationController;
    public Transform AITargetSitTransform;

    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private bool _isAIChef;
 
   

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
        var zort = Random.Range(0, 2);
        if (!_isAIChef)
        {
            AIChangeState(AITargetRestaurantState);

        }
        else
        {
            AIChangeState(AIChefState);

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
        TableAvailablePanel.Instance.RedAvailability(AIController.AIOwnerTable.TableNumber);
        AIController.AIOwnerTable._aiControllerList.Remove(AIController);
        AIController.DeactivatedFoodIcon();
        AIController.AIOwnerChair.isChairAvailable = true;
        AIController.AIOwnerTable.CustomerCount--;
        AIController.AIOwnerTable.TotalBills += GameDataManager.Instance.GetOrderBill(AIController.FoodDataStruct.OrderType);
        
        CheckOrderBillsPanel.Instance.UpdatePanel(AIController.AIOwnerTable.TableNumber,AIController.AIOwnerTable.TotalBills);    
        Destroy(AIController.AIOwnerFood.FoodObject);
        TableAvailablePanel.Instance.CheckTable(AIController.AIOwnerTable.TableNumber);
        AIController.AIOwnerTable.RemoveOrder(AIController.FoodDataStruct);
        if (AIController.AIOwnerTable.CheckAllCustomerFinishedFood())
        {
            AIController.AIOwnerTable.AllFoodfinished();
        }
        //AIChangeState(AIMoveState);
    }
}

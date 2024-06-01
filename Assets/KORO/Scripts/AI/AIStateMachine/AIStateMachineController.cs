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
    public AIController AIController;
    public AIAnimationController AIAnimationController;
    [SerializeField] private Animator _playerAnimator;
 
   

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
        AITargetSitState = new AITargetSitState(this);
        AITargetRestaurantState = new AITargetRestaurantState(this);
        AIWaitPlayerState = new AIWaitPlayerState(this);
        var zort = Random.Range(0, 2);
        AIChangeState(AITargetRestaurantState);


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
}

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
    public AIController AIController;
    
    [SerializeField] private Animator _playerAnimator;
 
   

    // Update is called once per frame
    void Update()
    {
        
    }

    void Start()
    {
        gameObject.TryGetComponent(out AIController);
        _playerAnimator = gameObject.GetComponent<Animator>();
        AIIdleState = new AIIdleState(this);
        AIClapState = new AIClapState(this);
        AIMoveState = new AIMoveState(this);
        AISitState = new AISitState(this);
        AIChangeState(AIMoveState);
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

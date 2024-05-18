using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{

    [SerializeField] private NavMeshAgent _agent;

    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Transform _targetFirstPosition;
    [SerializeField] private Transform _playerPosition;
    [SerializeField] private Transform _chairPosition;
    
    [SerializeField] private Animator _playerAnimator;
    public AIStateMachineController AIStateMachineController;
    
    public LayerMask obstacleMask;
    public int obstacleMaskValue;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.TryGetComponent(out AIStateMachineController);
    }
    /////////// SIT STATE ///////////
    public void StartSitState()
    {
        _agent.speed = 0f;
        _playerAnimator.Play("Sit",0);
    }
    /////////// MOVE STATE ///////////
    public void StartTargetDestination()
    {
        _agent.speed = 1f;
        _agent.destination = _targetTransform.position;
        float randomTime = Random.Range(0f, 1f);
        _playerAnimator.Play("Walk",0,randomTime);
        //obstacleMaskValue = LayerMask.NameToLayer("Player");
        InvokeRepeating("CheckForObstacles", 0f, 0.3f);
        //_targetFirstPosition = transform.position;
    }
    
    /////////// CLAP STATE ///////////
    public void StartClapState()
    {
        _agent.speed = 0;
        _playerAnimator.Play("Clapping",0,0);
        if (_playerPosition)
        {
            transform.LookAt(_playerPosition);
        }

        StartCoroutine(Walking());
    }

    public IEnumerator Walking()
    {
        yield return new WaitForSeconds(3f);
        
        AIStateMachineController.AIChangeState(AIStateMachineController.AIMoveState);
    }
    // Update is called once per frame
    void Update()
    {
        if (1 > Vector3.Distance(transform.position, _targetTransform.position))
        {
            transform.position = _targetFirstPosition.position;
        }
    }
    void CheckForObstacles()
    {
            RaycastHit hit;
            if (Physics.Raycast(transform.position+ Vector3.up, transform.right, out hit, 2f, obstacleMask))
            {
                var player = hit.collider.gameObject.GetComponent<Player>();
                if (player._canTakeMoney)
                {
                    player.GainMoney(1f);
                    _playerPosition = hit.collider.gameObject.GetComponent<Player>().transform;

                    AIStateMachineController.AIChangeState(AIStateMachineController.AIClapState);

                }
                Debug.DrawRay(transform.position + Vector3.up, transform.right * 2f, Color.blue);

            }
        
        
            if (Physics.Raycast(transform.position+ Vector3.up, -transform.right, out hit, 2f, obstacleMask))
            {
                var player = hit.collider.gameObject.GetComponent<Player>();
                if (player._canTakeMoney)
                {
                    player.GainMoney(1f);
                    _playerPosition = hit.collider.gameObject.GetComponent<Player>().transform;

                    AIStateMachineController.AIChangeState(AIStateMachineController.AIClapState);
                }   

                Debug.DrawRay(transform.position + Vector3.up, -transform.right * 2f, Color.red);

            }
            
        
    }
}

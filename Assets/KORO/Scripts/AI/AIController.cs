using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class AIController : MonoBehaviour,IInterectableObject
{
    
    
    public NavMeshAgent _agent;

    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Transform _targetFirstPosition;
    [SerializeField] private Transform _playerPosition;
    [SerializeField] private Transform _chairPosition;
    [SerializeField] private AICanvas _aıCanvas_;
    [SerializeField] private List<Transform> _targetPositions;
    
    [FormerlySerializedAs("_playerAnimator")] public Animator AiAnimator;
    public AIAnimationController AIAnimationController;
    public AIStateMachineController AIStateMachineController;
    
    public LayerMask obstacleMask;
    public int obstacleMaskValue;
    public int AgentID;
    public int destinationValue = -1;
    public Table AIOwnerTable;
    public Chair AIOwnerChair;
    public OrderDataStruct FoodDataStruct;

    // Start is called before the first frame update
    void Start()
    {
        destinationValue = -1;
        gameObject.TryGetComponent(out AIStateMachineController);
        gameObject.TryGetComponent(out AIAnimationController);
    }
    /////////// SIT STATE ///////////
    public void StartSitState()
    {
        _agent.speed = 0f;
        AiAnimator.Play("Sit",0);
        StartCoroutine(FoodIcon());

    }
    /////////// MOVE STATE ///////////
    public void SetDestinationTarget()
    {
        /*var zort = Random.Range(0, 2);
        if (zort == 0)
        {
            if (destinationValue != 0)
            {
                destinationValue = 0;
                _agent.destination = _targetPositions[destinationValue].position;

            }
            else
            {
                _targetTransform = _targetPositions[1];
                _agent.destination = _targetPositions[1].position;
                //GetComponent<AIAreaController>().InteractabelDeactive();
            }

        }
        else
        {
            if (destinationValue != 1)
            {
                destinationValue = 1;
                _agent.destination = _targetPositions[destinationValue].position;

            }
            else
            {
                destinationValue = 2;
                _agent.destination = _targetPositions[destinationValue].position;
            }
            _targetTransform = _targetPositions[destinationValue];

            //GetComponent<AIAreaController>().InteractabelDeactive();


        }*/
        _agent.destination = _targetPositions[1].position;
    }
    public void StartTargetDestination()
    {
        _agent.speed = 1f;
        SetDestinationTarget();
        float randomTime = Random.Range(0f, 1f);
        AiAnimator.Play("Walk",0,randomTime);
        //obstacleMaskValue = LayerMask.NameToLayer("Player");
        InvokeRepeating("CheckForObstacles", 0f, 0.3f);
        //_targetFirstPosition = transform.position;
    }
    
    /////////// CLAP STATE ///////////
    public void StartClapState()
    {
        _agent.speed = 0;
        AiAnimator.Play("Clapping",0,0);
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
    public IEnumerator FoodIcon()
    {
        yield return new WaitForSeconds(3f);
        
        _aıCanvas_.InfoImage.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (1 > Vector3.Distance(transform.position, _targetTransform.position))
        {
            StartTargetDestination();
            GetComponent<AIAreaController>().InteractabelDeactive();
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
    
    public void InterectableObjectRun()
    {
        if (PlayerOrderController.Instance.TakedFood)
        {
            PlayerOrderController.Instance.TakedFood = false;
            PlayerOrderController.Instance.Food.transform.position = AIOwnerChair.ChairFoodTransform.position;
            PlayerOrderController.Instance.Food.transform.rotation = AIOwnerChair.ChairFoodTransform.rotation;
            PlayerOrderController.Instance.Food.transform.SetParent(AIOwnerChair.ChairFoodTransform);
            AIStateMachineController.AIChangeState(AIStateMachineController.AIEatState);
        }
    }

    public void ShowOutline(bool active)
    {
        
    }

    public Outline GetOutlineComponent()
    {
        return null;
    }

    public string GetInterectableText()
    {
        if (PlayerOrderController.Instance.TakedFood)
        {
            return "Give Food";
        }
        else
        {
            return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class AIController : MonoBehaviour,IInterectableObject
{
    
    
    public NavMeshAgent _agent;

    public Transform _targetTransform;
    public Transform _targetFirstPosition;
    public bool IsFinishedFood;
    public bool IsSitting;
    public bool IsTakedFood;
    [SerializeField] private Transform _playerPosition;
    [SerializeField] private Transform _chairPosition;
    [SerializeField] private AICanvas _aıCanvas_;
    public List<Transform> _targetPositions;
    public List<GameObject> AIModels;
    //public List<AIController>
    [FormerlySerializedAs("_playerAnimator")] public Animator AiAnimator;
    public AIAnimationController AIAnimationController;
    public AIStateMachineController AIStateMachineController;
    
    public LayerMask obstacleMask;
    public int obstacleMaskValue;
    public int AgentID;
    public int destinationValue = -1;
    public Table AIOwnerTable;
    public Chair AIOwnerChair;
    public FoodTable AIOwnerFood;
    public OrderDataStruct FoodDataStruct;

    
    // Start is called before the first frame update
    public void Initiliaze(bool isFriend = false)
    {
        destinationValue = -1;
        gameObject.TryGetComponent(out AIStateMachineController);
        gameObject.TryGetComponent(out AIAnimationController);
        AIStateMachineController.Initialize(isFriend);
    }
    /////////// SIT STATE ///////////
    public void StartSitState()
    {
        _agent.speed = 0f;
        AiAnimator.Play("Sit",0);
        //StartCoroutine(FoodIcon());

    }
    /////////// MOVE STATE ///////////
    public void SetDestinationTarget(Vector3 targetPosition)
    {
        
        _agent.destination = targetPosition;
    }
    public void StartTargetDestination(Vector3 targetPosition)
    {
        _agent.speed = 1f;
        SetDestinationTarget(targetPosition);
        float randomTime = Random.Range(0f, 1f);
        AiAnimator.Play("Walk",0,randomTime);
        //obstacleMaskValue = LayerMask.NameToLayer("Player");
        //InvokeRepeating("CheckForObstacles", 0f, 0.3f);
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
        yield return new WaitForSeconds(0.2f);

        if (!_aıCanvas_.InfoImage.gameObject.activeSelf)
        {
            _aıCanvas_.InfoImage.gameObject.SetActive(true);
            _aıCanvas_.InfoImage.sprite = GameDataManager.Instance.GetFoodSprite(FoodDataStruct.OrderType);

        }
        
    }

    public void DeactivatedFoodIcon()
    {
        if (_aıCanvas_.InfoImage.gameObject.activeSelf)
        {
            _aıCanvas_.InfoImage.gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        /*if (_targetTransform)
        {
            /*if (1 > Vector3.Distance(transform.position, _targetTransform.position))
            {
                StartTargetDestination();
                GetComponent<AIAreaController>().InteractabelDeactive();
                transform.position = _targetFirstPosition.position;
            }#1#
        }*/
        
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
        if (PlayerOrderController.Instance.TakedFood && PlayerOrderController.Instance.Food.OrderType == FoodDataStruct.OrderType && IsSitting)
        {
            IsTakedFood = true;
            AIOwnerFood = PlayerOrderController.Instance.FoodTable;
            PlayerOrderController.Instance.TakedFood = false;
            //PlayerOrderController.Instance.FoodTable.transform.position = AIOwnerChair.ChairFoodTransform.position;
            //PlayerOrderController.Instance.FoodTable.transform.rotation = AIOwnerChair.ChairFoodTransform.rotation;
            PlayerOrderController.Instance.FoodTable.transform.SetParent(AIOwnerChair.ChairFoodTransform);

            PlayerOrderController.Instance.FoodTable.transform.DORotate(AIOwnerChair.ChairFoodTransform.rotation.eulerAngles,0.2f);
            PlayerOrderController.Instance.FoodTable.transform.DOMove(AIOwnerChair.ChairFoodTransform.position,0.2f);

            PlayerOrderController.Instance.FoodTable = null;
            AIStateMachineController.AIChangeState(AIStateMachineController.AIEatState);
        }
    }
    public void InterectableObjectRunforWaiter(FoodTable foodTable)
    {
        IsTakedFood = true;
        AIOwnerFood = foodTable;
        foodTable.transform.position = AIOwnerChair.ChairFoodTransform.position;
        foodTable.transform.rotation = AIOwnerChair.ChairFoodTransform.rotation;
        foodTable.transform.SetParent(AIOwnerChair.ChairFoodTransform);
            
        AIStateMachineController.AIChangeState(AIStateMachineController.AIEatState);
        
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
    public void Move()
    {
        
    }
    public void SetModel(int modelIndex)
    {
        for (int i = 0; i < AIModels.Count; i++)
        {
            if (modelIndex == i)
            {
                AIModels[i].SetActive(true);
            }
        }
    }
}

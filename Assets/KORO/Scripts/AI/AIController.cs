using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour,IInterectableObject
{
    
    
    public NavMeshAgent _agent;

    
    [SerializeField] private string[] texts = new [] {"Take OrderBox"};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    public Transform _targetTransform;
    public Transform _targetFirstPosition;
    public bool IsFinishedFood;
    public bool IsSitting;
    public bool IsTakedFood;
    public bool IsBadGuy;
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

    private Outline _outline;
    
    // Start is called before the first frame update
    public void Initiliaze(bool isFriend = false)
    {
        destinationValue = -1;
        gameObject.TryGetComponent(out AIStateMachineController);
        gameObject.TryGetComponent(out AIAnimationController);
        AIStateMachineController.Initialize(isFriend);
        _outline = GetComponent<Outline>();

    }

    private void Start()
    {
        texts = new [] {"Serv the food"};
        textsButtons = new [] {"E"};
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
        if (TryGetComponent(out AIRagdollController aiRagdollController))
        {
            if (aiRagdollController)
            {
                StartCoroutine(aiRagdollController.AddForceToAICor(PlayerOrderController.Instance.transform.forward));

                aiRagdollController.SetRagdollState(true);
                //aiRagdollController.AddForceToAI(PlayerOrderController.Instance.transform.forward);
            }
        }
        if (PlayerOrderController.Instance.TakedFood && PlayerOrderController.Instance.Food.OrderType == FoodDataStruct.OrderType && IsSitting)
        {
            IsTakedFood = true;
            AIOwnerFood = PlayerOrderController.Instance.FoodTable;
            AIOwnerFood.FoodGivedCustomer();
            var playerOrderController = PlayerOrderController.Instance;
            var foodTable = playerOrderController.FoodTable;
            playerOrderController.TakedFood = false;
            Player.Instance.PlayerStateType = Enums.PlayerStateType.Free;
            //PlayerOrderController.Instance.FoodTable.transform.position = AIOwnerChair.ChairFoodTransform.position;
            //PlayerOrderController.Instance.FoodTable.transform.rotation = AIOwnerChair.ChairFoodTransform.rotation;
            foodTable.transform.SetParent(AIOwnerChair.ChairFoodTransform);

            foodTable.transform.DORotate(AIOwnerChair.ChairFoodTransform.rotation.eulerAngles,0.2f);
            foodTable.transform.DOMove(AIOwnerChair.ChairFoodTransform.position,0.2f);
            foodTable = null;
            playerOrderController.FoodTable = null;
            Player.Instance.DropTakenObject();
            AIStateMachineController.AIChangeState(AIStateMachineController.AIEatState);
            if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 4)
            {
                TutorialManager.Instance.SetTutorialInfo(9);
            }
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
        if (PlayerOrderController.Instance.TakedFood && PlayerOrderController.Instance.Food.OrderType == FoodDataStruct.OrderType && IsSitting)
        {
            _outline.enabled = active;
        }
    }

    public Outline GetOutlineComponent()
    {
        if (PlayerOrderController.Instance.TakedFood && PlayerOrderController.Instance.Food.OrderType == FoodDataStruct.OrderType && IsSitting)
        {
            return _outline;
        }
        return null;
    }

    public string GetInterectableText()
    {
        if (PlayerOrderController.Instance.TakedFood)
        {
            return "Serv The Food";
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
            else
            {
                AIModels[i].SetActive(false);
            }
        }
    }
    public void Open()
    {
        
    }
    public string[] GetInterectableTexts()
    {
        //return null;
        if (PlayerOrderController.Instance.TakedFood)
        {
            return texts;
        }
        else
        {
            return null;
        }
    }
    public string[] GetInterectableButtons()
    {
        
        if (PlayerOrderController.Instance.TakedFood)
        {
            return textsButtons;
        }
        else
        {
            return null;
        }
    }
    public Enums.PlayerStateType GetStateType()
    {
        if (IsBadGuy)
        {
            return Enums.PlayerStateType.Fight;
        }
        else
        {
            return Enums.PlayerStateType.GiveFood;
        }
    }
}

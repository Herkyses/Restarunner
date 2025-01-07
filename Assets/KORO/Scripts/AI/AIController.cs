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
    public Transform TargetTransform;
    public Transform TargetFirstPosition;
    public bool IsFinishedFood;
    public bool IsSitting;
    public bool IsTakedFood;
    public bool IsBadGuy;
    
    [SerializeField] private Transform _playerPosition;
    [SerializeField] private Transform _chairPosition;
    [SerializeField] private AICanvas _aıCanvas_;
    [SerializeField] private string[] texts = new [] {"Take OrderBox"};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    
    
    public List<Transform> _targetPositions;
    public GameObject AIModel;
    [FormerlySerializedAs("_playerAnimator")] public Animator AiAnimator;
    public AIAnimationController AIAnimationController;
    public AIStateMachineController AIStateMachineController;
    
    public int AgentID;
    public int DestinationValue = -1;
    public int customerPatienceRate = -1;
    public float CustomerPatienceRate = 7;
    private Player _player;
    private Outline _outline;
    
    public LayerMask ObstacleMask;
    public int ObstacleMaskValue;
    
    public Table AIOwnerTable;
    public Chair AIOwnerChair;
    public FoodTable AIOwnerFood;
    public OrderDataStruct FoodDataStruct;
    public CustomerData CustomerData;
    


    

    // Start is called before the first frame update
    public void Initiliaze(bool isFriend = false)
    {
        DestinationValue = -1;
        gameObject.TryGetComponent(out AIStateMachineController);
        gameObject.TryGetComponent(out AIAnimationController);
        AIStateMachineController.Initialize(isFriend);
        _outline = GetComponent<Outline>();

    }

    private void Start()
    {
        texts = new [] {"Key_Serve_Food"};
        textsButtons = new [] {"E"};
        _player = Player.Instance;
        //IsBadGuy = true;
    }

    /////////// SIT STATE ///////////
    public void StartSitState()
    {
        //_agent.speed = 0f;
        AiAnimator.Play("Sit",0);
        //StartCoroutine(FoodIcon());

    }
    /////////// MOVE STATE ///////////
    public void SetDestinationTarget(Vector3 targetPosition)
    {
        
        _agent.destination = targetPosition;
        _agent.enabled = true;

    }
    public void StartTargetDestination(Vector3 targetPosition)
    {
        _agent.enabled = true;
        _agent.speed = 1f;
        SetDestinationTarget(targetPosition);
        float randomTime = Random.Range(0f, 1f);
        AiAnimator.Play("Walk",0,randomTime);
    }
    /////////// RUN STATE ///////////
    
    public void StartTargetDestinationForRun(Vector3 targetPosition)
    {
        _outline.enabled = true;
        _outline.OutlineColor = Color.red;
        _agent.enabled = true;

        _agent.speed = 3.5f;
        SetDestinationTarget(targetPosition);
        float randomTime = Random.Range(0f, 1f);
        AiAnimator.Play("Run",0,randomTime);
        AISpawnController.CatchNonPayer?.Invoke();
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
    
    
    
    public void InterectableObjectRun()
    {
        if (TryGetComponent(out AIRagdollController aiRagdollController) && IsBadGuy)
        {
            HandleBadGuyInteraction(aiRagdollController);
        }
        if (_player.PlayerOrdersController.TakedFood && _player.PlayerOrdersController.Food.OrderType == FoodDataStruct.OrderType && IsSitting && !IsTakedFood)
        {
            HandleFoodInteraction();
        }
    }
    private void HandleBadGuyInteraction(AIRagdollController ragdollController)
    {
        StartCoroutine(ragdollController.AddForceToAICor(_player.PlayerOrdersController.transform.forward));
        _player.StartFight();
        GameVfxManager.Instance.SpawnVFX(GameVfxManager.Instance.vfxPools[0].vfxPrefab, CameraController.Instance.BoomVFXTransformParent.position, CameraController.Instance.BoomVFXTransformParent.rotation);
        ragdollController.SetRagdollState(true);
        
        if (IsBadGuy)
        {
            ragdollController.ResetBadGuy();
            var cash = PoolManager.Instance.GetFromPoolForCash();
            cash.GetComponent<Cash>().Initiliaze(GameDataManager.Instance.GetFoodPrice(AIOwnerFood.OrderType));
            cash.transform.position = transform.position;
        }
    }
    private void HandleFoodInteraction()
    {
        IsTakedFood = true;
        AIOwnerFood = _player.PlayerOrdersController.FoodTable;
        AIOwnerFood.FoodGivedCustomer();
        AssignFoodToAI(AIOwnerChair);
        AISpawnController.AITakedFood?.Invoke();
        AIStateMachineController.AIChangeState(AIStateMachineController.AIEatState);
        HandleTutorialStep();
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
    
    
    private void AssignFoodToAI(Chair aiOwnerChair)
    {
        IsTakedFood = true;
        AIOwnerFood = PlayerOrderController.Instance.FoodTable;
        AIOwnerFood.FoodGivedCustomer();
    
        MoveFoodToAIChair(AIOwnerFood, aiOwnerChair);
    }
    private void MoveFoodToAIChair(FoodTable foodTable, Chair aiOwnerChair)
    {
        const float moveDuration = 0.2f;

        foodTable.transform.SetParent(aiOwnerChair.ChairFoodTransform);
        foodTable.transform.DORotate(aiOwnerChair.ChairFoodTransform.rotation.eulerAngles, moveDuration);
        foodTable.transform.DOMove(aiOwnerChair.ChairFoodTransform.position, moveDuration);
    }
    private void HandleTutorialStep()
    {
        if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 5)
        {
            TutorialManager.Instance.SetTutorialInfo(10);
        }
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
            return "Key_Serve_Food";
        }
        else
        {
            return null;
        }
    }


    public void CalculatePopularityRate()
    {
        //var customerPatienceRate = Random.Range(1, 11);
        customerPatienceRate = AIStateMachineController.AIWaitTimeController.WaitTimeTempValue;
        PopularityManager.Instance.CalculateSinglePopularityValue(customerPatienceRate,AIOwnerFood.GetWaitFoodTime(),AIOwnerTable.TableQuality);
    }
    public void SetTableInfo(Table table, Chair chair)
    {
        AIOwnerTable = table;
        AIOwnerChair = chair;
        IsSitting = true;
    }
    public void SetModel()
    {
        var modelIndex = Random.Range(0, CustomerData.CharacterModels.Count);
        AIModel.GetComponent<SkinnedMeshRenderer>().sharedMesh = CustomerData.CharacterModels[modelIndex];
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
    public void AssignToChair(AIAreaController aiArea, Transform chair)
    {
        aiArea.AIController._agent.speed = 0;
        aiArea.AIController._agent.enabled = false;
        aiArea.transform.position = chair.position;
        aiArea.transform.rotation = chair.rotation;

        var stateMachineController = aiArea.GetComponent<AIStateMachineController>();
        stateMachineController.AIChangeState(stateMachineController.AISitState);

        aiArea.InteractabelControl();
        //_aiControllerList.Add(aiArea.GetComponent<AIController>());
        
        //aiArea.AIController.SetTableInfo(this, chair);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AISpawnController : MonoBehaviour
{
    public List<Transform> TargetList;
    public List<AIController> AllAIList;
    public AIController AlPf;
    public AIController AlPfRagdoll;
    public static Action CatchNonPayer;
    public static Action AITakedFood;

    public int ActiveAiCount;

    private void OnEnable()
    {
        OpenCloseController.RestaurantOpened += SetRestaurantStateForTutorial;
        GameSceneCanvas.UpdateAISpawnController += UpdateAiSpawner;
        PoolManager.IsPoolManagerInitiliazed += InitiliazeAIS;
    }
    private void OnDisable()
    {
        OpenCloseController.RestaurantOpened -= SetRestaurantStateForTutorial;
        GameSceneCanvas.UpdateAISpawnController -= UpdateAiSpawner;
        PoolManager.IsPoolManagerInitiliazed -= InitiliazeAIS;

    }


    public static AISpawnController Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        //ActiveAiCount = 4;
    }

    public void UpdateAiSpawner()
    {
        ActiveAiCount++;
        NewAISpawn();
    }

    public void SetRestaurantStateForTutorial()
    {
        Debug.Log("tutorialstep" + PlayerPrefsManager.Instance.LoadPlayerTutorialStep());
        AllAIList[0].AIStateMachineController.AIChangeState(AllAIList[0].AIStateMachineController.AITargetRestaurantState);
        if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 5)
        {
            AllAIList[0].AIStateMachineController.AIChangeState(AllAIList[0].AIStateMachineController.AITargetRestaurantState);
        }
    }
    public void NewAISpawn()
    {
        var singleAi = PoolManager.Instance.GetCustomerRagdollAI().GetComponent<AIController>(); 
        
        InitializeSingleAI(singleAi,ActiveAiCount);
    }
    private void InitializeSingleAI(AIController singleAi, int agentId, List<AIController> friends = null, Transform spawnTransform = null)
    {
        singleAi.transform.SetParent(transform);
        singleAi.AgentID = agentId;

        singleAi.SetModel();
        AllAIList.Add(singleAi);

        if (friends != null)
        {
            friends.Add(singleAi);
            singleAi.transform.position = spawnTransform.position + Vector3.back * 0.2f;
            SetTransformToAI(singleAi, true);
        }
        else
        {
            SetTransformToAI(singleAi);
        }

        singleAi.Initiliaze(friends != null);
    }

    public void SetTransformForAI(AIController singleAi)
    {
        SetTransformToAI(singleAi); 
        singleAi.AIStateMachineController.AIInitialize();
    }

    public void InitiliazeAIS()
    {
        StartCoroutine(Initialize());
    }
    public IEnumerator Initialize()
    {
        int initialAiCount = ActiveAiCount + PlayerPrefsManager.Instance.LoadCustomerCount() / 2;
        for (int i = 0; i < initialAiCount; i++)
        {
            var ranDomTime = Random.Range(1, 5);
            yield return new WaitForSeconds(ranDomTime);
            //var singleAi = Instantiate(AlPf,transform);
            var singleAi = new AIController();
            singleAi = PoolManager.Instance.GetCustomerRagdollAI().GetComponent<AIController>();
            
            InitializeSingleAI(singleAi, i);

        }
    }

    public void CreateAIForGroup(List<AIController> friends,Transform spawnTransform)
    {


        var table = GetAvailableTable();

        if (table != null)
        {
            for (int i = 0; i < table.TableCapacity - 1; i++)
            {
                var singleAi = PoolManager.Instance.GetCustomerRagdollAI().GetComponent<AIController>();
                InitializeSingleAI(singleAi, AllAIList.Count, friends, spawnTransform);
            }
        }
        
    }
    // Uygun masa bulur, yoksa null döner
    private Table GetAvailableTable()
    {
        foreach (var tableSet in ControllerManager.Instance.Tablecontroller.TableSets)
        {
            if (tableSet.table.IsTableAvailable)
            {
                return tableSet.table;
            }
        }
        return null;
    }
    public void GetAvailableAI(int aiCount)
    {
        
    }

    
    public void SetTransformToAI(AIController singleAi,bool isFriend = false)
    {
        
        var randomOffset = Random.Range(-2, 2);
        var primaryTarget = Random.value < 0.5f ? TargetList[0] : TargetList[1];
        var secondaryTarget = primaryTarget == TargetList[0] ? TargetList[1] : TargetList[0];

        if (!isFriend)
        {
            singleAi.transform.position = primaryTarget.position + Vector3.right * randomOffset;
        }

        singleAi.TargetTransform = secondaryTarget;
        singleAi.TargetFirstPosition = primaryTarget;
        singleAi._targetPositions[1] = primaryTarget;
        singleAi._targetPositions[2] = secondaryTarget;
    }
    
}

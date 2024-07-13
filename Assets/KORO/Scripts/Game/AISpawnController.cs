using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AISpawnController : MonoBehaviour
{
    public List<Transform> TargetList;
    public List<AIController> AllAIList;
    public AIController AlPf;

    public int ActiveAiCount;

    private void OnEnable()
    {
        GameSceneCanvas.UpdateAISpawnController += UpdateAiSpawner;
    }
    private void OnDisable()
    {
        GameSceneCanvas.UpdateAISpawnController -= UpdateAiSpawner;
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

    
    public void NewAISpawn()
    {
        var singleAi = Instantiate(AlPf,transform);
        var index = Random.Range(0, 7);
        singleAi.SetModel(index);
        AllAIList.Add(singleAi);
        singleAi.AgentID = ActiveAiCount;
        SetTransformToAI(singleAi);
    }

    public void SetTransformForAI(AIController singleAi)
    {
        SetTransformToAI(singleAi); 
        singleAi.AIStateMachineController.AIInitialize();
    }
    public IEnumerator Initialize()
    {
        for (int i = 0; i < ActiveAiCount + (PlayerPrefsManager.Instance.LoadPopularity()/2); i++)
        {
            var ranDomTime = Random.Range(1, 3);
            yield return new WaitForSeconds(ranDomTime);
            var singleAi = Instantiate(AlPf,transform);
            var index = Random.Range(0, 7);
            singleAi.SetModel(index);
            AllAIList.Add(singleAi);
            singleAi.AgentID = i;
            SetTransformToAI(singleAi);
            singleAi.Initiliaze();

        }
    }

    public void CreateAIForGroup(List<AIController> friends,Transform spawnTransform)
    {
        /*var singleAi = Instantiate(AlPf,transform);
        var index = Random.Range(0, 7);
        singleAi.SetModel(index);
        singleAi.transform.position = spawnTransform.position + Vector3.back * 0.2f;
        singleAi.AgentID = AllAIList.Count;
        AllAIList.Add(singleAi);
        singleAi.Initiliaze(true);*/
        var randomTable = 0;
        var table = new Table();
        for (int i = 0; i < TableController.Instance.TableSets.Count; i++)
        {
            if (TableController.Instance.TableSets[i].table.IsTableAvailable)
            {
                randomTable = TableController.Instance.TableSets[i].table.TableNumber;
                table = TableController.Instance.TableSets[i].table;
                break;
            }
            
            randomTable = -1;
            
        }

        if (randomTable != -1)
        {
            for (int i = 0; i < table.TableCapacity-1; i++)
            {
                var ranDomTime = Random.Range(1, 3);
                var singleAi = Instantiate(AlPf,transform);
                var index = Random.Range(0, 7);
                singleAi.SetModel(index);
                singleAi.transform.position = spawnTransform.position + Vector3.back * 0.2f;
                AllAIList.Add(singleAi);
                friends.Add(singleAi);
                singleAi.AgentID = AllAIList.Count;
                singleAi.Initiliaze(true);
                SetTransformToAI(singleAi,true);
            }
        }
        
    }

    public void GetAvailableAI(int aiCount)
    {
        
    }
    public void SetTransformToAI(AIController singleAi,bool isFriend = false)
    {
        
        if (Random.value < 0.5f)
        {
            if (!isFriend)
            {
                singleAi.transform.position = TargetList[0].transform.position;
            }
            singleAi._targetTransform = TargetList[1].transform;
            singleAi._targetFirstPosition = TargetList[0].transform;
            singleAi._targetPositions[1] = TargetList[0].transform;
            singleAi._targetPositions[2] = TargetList[1].transform;        
        }
        else
        {
            if (!isFriend)
            {
                singleAi.transform.position = TargetList[1].transform.position;
            }
            singleAi._targetTransform = TargetList[0].transform;
            singleAi._targetFirstPosition = TargetList[1].transform;
            singleAi._targetPositions[1] = TargetList[1].transform;
            singleAi._targetPositions[2] = TargetList[0].transform;        
        }
    }
    
}

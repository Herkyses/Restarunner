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
        var randomPosition = Random.Range(0, 2);
            
            
        if (randomPosition == 0)
        {
            singleAi.transform.position = TargetList[0].transform.position;
            singleAi._targetTransform = TargetList[1].transform;
            singleAi._targetFirstPosition = TargetList[0].transform;
            singleAi._targetPositions[1] = TargetList[0].transform;
            singleAi._targetPositions[2] = TargetList[1].transform;
        }
        else
        {
            singleAi.transform.position = TargetList[1].transform.position;
            singleAi._targetTransform = TargetList[0].transform;
            singleAi._targetFirstPosition = TargetList[1].transform;
            singleAi._targetPositions[1] = TargetList[1].transform;
            singleAi._targetPositions[2] = TargetList[0].transform;

        }
    }

    public void SetTransformForAI(AIController singleAi)
    {
        var randomPosition = Random.Range(0, 2);
            
            
        if (randomPosition == 0)
        {
            singleAi.transform.position = TargetList[0].transform.position;
            singleAi._targetTransform = TargetList[1].transform;
            singleAi._targetFirstPosition = TargetList[0].transform;
            singleAi._targetPositions[1] = TargetList[0].transform;
            singleAi._targetPositions[2] = TargetList[1].transform;
        }
        else
        {
            singleAi.transform.position = TargetList[1].transform.position;
            singleAi._targetTransform = TargetList[0].transform;
            singleAi._targetFirstPosition = TargetList[1].transform;
            singleAi._targetPositions[1] = TargetList[1].transform;
            singleAi._targetPositions[2] = TargetList[0].transform;

        }  
        singleAi.AIStateMachineController.AIInitialize();
    }
    public void Initialize()
    {
        for (int i = 0; i < ActiveAiCount + (PlayerPrefsManager.Instance.LoadPopularity()/2); i++)
        {
            var singleAi = Instantiate(AlPf,transform);
            var index = Random.Range(0, 7);
            singleAi.SetModel(index);
            AllAIList.Add(singleAi);
            singleAi.AgentID = i;
            var randomPosition = Random.Range(0, 2);
            
            
            if (randomPosition == 0)
            {
                singleAi.transform.position = TargetList[0].transform.position;
                singleAi._targetTransform = TargetList[1].transform;
                singleAi._targetFirstPosition = TargetList[0].transform;
                singleAi._targetPositions[1] = TargetList[0].transform;
                singleAi._targetPositions[2] = TargetList[1].transform;
            }
            else
            {
                singleAi.transform.position = TargetList[1].transform.position;
                singleAi._targetTransform = TargetList[0].transform;
                singleAi._targetFirstPosition = TargetList[1].transform;
                singleAi._targetPositions[1] = TargetList[1].transform;
                singleAi._targetPositions[2] = TargetList[0].transform;

            }
        }
    }
}

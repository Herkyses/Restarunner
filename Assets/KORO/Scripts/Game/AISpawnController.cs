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

    public int AiCount;


    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        for (int i = 0; i < AiCount; i++)
        {
            var singleAi = Instantiate(AlPf,transform);
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

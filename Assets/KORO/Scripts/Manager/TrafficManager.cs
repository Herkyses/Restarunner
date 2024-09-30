using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class TrafficManager : MonoBehaviour
{
    [SerializeField] private List<SingleCar> _cars;
    [SerializeField] private List<SingleCar> _spawnedCars;
    public List<Transform> _carSpawnTransforms;
    public List<WayPoint> WayPoints;
    public static TrafficManager Instance;

    public NavMeshSurface NavMeshSurfaceMap;
    // Start is called before the first frame update
    
    
    
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
        
    }

    public void Initiliaze()
    {
        for (int i = 0; i < WayPoints.Count; i++)
        {
            var car = Instantiate(_cars[i], transform);
            //car.transform.position = WayPoints[i].WayPoints[0].transform.position;
            car.Initiliaze(WayPoints[i]);
        }
        
        /*NavMesh.RemoveAllNavMeshData();
        NavMeshSurfaceMap.BuildNavMesh();*/
    }

    public void SetDestination(NavMeshAgent navMeshAgent)
    {
        navMeshAgent.destination = _carSpawnTransforms[1].position;
    }
}

[Serializable]
public class WayPoint
{
    public List<Transform> WayPoints;
    public List<TrafficLight> TrafficLights;
}

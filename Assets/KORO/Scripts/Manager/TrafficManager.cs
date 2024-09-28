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
        var car = Instantiate(_cars[0], transform);
        car.transform.position = WayPoints[0].WayPoints[0].transform.position;
        car.Initiliaze(WayPoints[0]);
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

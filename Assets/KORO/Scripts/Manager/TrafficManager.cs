using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrafficManager : MonoBehaviour
{
    [SerializeField] private List<SingleCar> _cars;
    [SerializeField] private List<SingleCar> _spawnedCars;
    [SerializeField] private List<Transform> _carSpawnTransforms;
    public static TrafficManager Instance;
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
    public void Initiliaze()
    {
        var car = Instantiate(_cars[0], transform);
        car.transform.position = _carSpawnTransforms[0].position;
        car.Initiliaze();
    }

    public void SetDestination(NavMeshAgent navMeshAgent)
    {
        navMeshAgent.destination = _carSpawnTransforms[1].position;
    }
}

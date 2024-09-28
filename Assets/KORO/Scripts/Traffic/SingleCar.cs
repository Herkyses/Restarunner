using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class SingleCar : MonoBehaviour
{
    [SerializeField] private List<Transform> _wheels;
    [SerializeField] private List<Tween> _wheelTweens = new List<Tween>();

    public NavMeshAgent CarNavMeshAgent;

    public WayPoint CarWayPoint;
    
    public float speed = 5f;
    public int currentWaypointIndex = 0;
    // Start is called before the first frame update
    public void Initiliaze(WayPoint wayPoint)
    {
        //StartWheelRotation();
        CarWayPoint = wayPoint;
        transform.rotation = Quaternion.Euler(transform.forward);
    }

    private void Start()
    {
        CarNavMeshAgent = GetComponent<NavMeshAgent>();
        StartWheelRotation();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            setss();
        }
        MoveToNextWaypoint();
    }

    public void StartWheelRotation()
    {
        _wheelTweens.Clear();
        for (int i = 0; i < _wheels.Count; i++)
        {
            _wheelTweens.Add(_wheels[i].DOLocalRotate(new Vector3(360f, 0, 0), 0.3f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1));
        }
        //TrafficManager.Instance.SetDestination(CarNavMeshAgent);
        Debug.Log("name" + gameObject.name);
        /*if (CarNavMeshAgent.isActiveAndEnabled)
        {
            Debug.Log("CarNavMeshAgent" + CarNavMeshAgent + "sss " + TrafficManager.Instance._carSpawnTransforms[1] );
            CarNavMeshAgent = GetComponent<NavMeshAgent>();
            CarNavMeshAgent.destination = TrafficManager.Instance._carSpawnTransforms[1].position;

        }*/

    }

    public void setss()
    {
        CarNavMeshAgent.destination = TrafficManager.Instance._carSpawnTransforms[1].position;
    }
    

    public void StopWheelRotation()
    {
        if (_wheelTweens.Count > 0)
        {
            for (int i = 0; i < _wheelTweens.Count; i++)
            {
                //_wheelTweens[i].SetLoops(0);
                _wheelTweens[i].Pause();
                _wheels[i].DOLocalRotate(new Vector3(_wheels[i].eulerAngles.x + 360f, 0, 0), 0.5f, RotateMode.FastBeyond360);
            }  
        }
        
    }
    public void MoveToNextWaypoint()
    {
        if (currentWaypointIndex >= CarWayPoint.WayPoints.Count) return;

        Vector3 targetPosition = CarWayPoint.WayPoints[currentWaypointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 1f)
        {
            currentWaypointIndex++;
        }
    }
}

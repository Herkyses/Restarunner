using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SingleAIPedestrianController : MonoBehaviour
{
    [SerializeField] private List<Transform> _wayPoints;
    [SerializeField] private Transform _target;
    [SerializeField] private NavMeshAgent _agent;
    private int currentPointIndex = 0;


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void Initiliaze(List<Transform> wayPointTransforms)
    {
        _wayPoints = wayPointTransforms;
    }

    public void SetTargetDestination(Transform targetTransform)
    {
        GetComponent<NavMeshAgent>().destination = targetTransform.position;
    }
    private void Update()
    {
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            MoveToNextPoint();
        }
    }

    void MoveToNextPoint()
    {
        if (_wayPoints.Count == 0) return;
        _agent.destination = _wayPoints[currentPointIndex].position;
        currentPointIndex = (currentPointIndex + 1) % _wayPoints.Count; // Sırayla noktalar arasında dön
    }
}

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
    [SerializeField] private Animator _animator;
    private int currentPointIndex = 0;


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _animator.Play("Walk",0);

    }

    public void Initiliaze(List<Transform> wayPointTransforms)
    {
        transform.position = wayPointTransforms[0].position;
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
        //currentPointIndex = (currentPointIndex + 1) % _wayPoints.Count;

        currentPointIndex++;
        // Sırayla noktalar arasında dön
        if (_wayPoints.Count == currentPointIndex)
        {
            transform.position = _wayPoints[0].position;
            currentPointIndex = 0;
        }
    }
}

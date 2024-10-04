using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SingleAIPedestrianController : MonoBehaviour
{
    [SerializeField] private List<Transform> _wayPoints;
    [SerializeField] private Vector3 _targetPosition = new Vector3();
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private bool _characterInited;
    [SerializeField] private bool _characterRorated;
    private int currentPointIndex = 0;
    private int _speed = 1;


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _animator.Play("Walk",0);
        _speed = 1;
    }

    public void Initiliaze(List<Transform> wayPointTransforms,int transformIndex)
    {
        var randomValue = Random.Range(-1, 1);
        transform.position = wayPointTransforms[transformIndex].position + Vector3.right*randomValue;


        if (transformIndex > ((float)(wayPointTransforms.Count - 1)/2))
        {
            wayPointTransforms.Reverse();
            var z = transformIndex;
            transformIndex = wayPointTransforms.Count - z;
        }
        else
        {
            transformIndex++;
        }
        currentPointIndex = transformIndex;
        _wayPoints = wayPointTransforms;
        _targetPosition = _wayPoints[currentPointIndex].position;
        
        _characterInited = true;
        transform.LookAt(_targetPosition);

    }

    public void SetTargetDestination(Transform targetTransform)
    {
        GetComponent<NavMeshAgent>().destination = targetTransform.position;
    }
    private void Update()
    {
        if (!_characterInited)
        {
            return;
        }
        /*if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            MoveToNextPoint();
        }*/
        _targetPosition = _wayPoints[currentPointIndex].position;

        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
        if (!_characterRorated)
        {
            _characterRorated = true;
            transform.LookAt(_targetPosition);
        }
        //

        if (HasReachedTarget())
        {
            currentPointIndex++;
            
            if (currentPointIndex == _wayPoints.Count)
            {
                currentPointIndex = 0;
                transform.position = _wayPoints[0].position;
            }
            _characterRorated = false;
            
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
    private bool HasReachedTarget()
    {
        return Vector3.Distance(transform.position, _targetPosition) < 1f;
    }

}

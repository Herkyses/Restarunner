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
    public bool checkable = true;
    public bool rotateable = false;
    public bool startedRotation = false;
    public Vector3 targetPosition ;
    // Start is called before the first frame update
    public void Initiliaze(WayPoint wayPoint)
    {
        speed = 8f;
        checkable = true;
        rotateable = false;
        startedRotation = false;
        //StartWheelRotation();
        CarWayPoint = wayPoint;
        currentWaypointIndex = 0;
        transform.position = wayPoint.WayPoints[0].transform.position;
        transform.rotation = Quaternion.Euler(transform.forward);
    }

    private void Start()
    {
        StartWheelRotation();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (CarWayPoint.TrafficLights[0] && CarWayPoint.TrafficLights[0].currentState == TrafficLight.LightState.Red && IsNear())
        {
            // Kırmızı ışıkta dur
            StopWheelRotation();
            return;
        }
        StartWheelRotation();
        MoveToNextWaypoint();
    }

    public void StartWheelRotation()
    {
        if (!startedRotation)
        {
            startedRotation = true;
            _wheelTweens.Clear();
            for (int i = 0; i < _wheels.Count; i++)
            {
                _wheelTweens.Add(_wheels[i].DOLocalRotate(new Vector3(360f, 0, 0), 0.3f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1));
            }
            Debug.Log("name" + gameObject.name);
        }
    }
    
    

    public void StopWheelRotation()
    {
        if (startedRotation)
        {
            startedRotation = false;
            if (_wheelTweens.Count > 0)
            {

                for (int i = 0; i < _wheelTweens.Count; i++)
                {
                    //_wheelTweens[i].SetLoops(0);
                    _wheelTweens[i].Pause();
                    _wheels[i].DOLocalRotate(new Vector3(_wheels[i].eulerAngles.x + 360f, 0, 0), 0.5f, RotateMode.FastBeyond360);
                }

                //startedRotation = false;
            }
        }
        
        
    }
    public void MoveToNextWaypoint()
    {
        if (currentWaypointIndex >= CarWayPoint.WayPoints.Count) return;

        targetPosition = CarWayPoint.WayPoints[currentWaypointIndex].position;
        
        
        MoveToTarget();
        
        if (HasReachedTarget())
        {
            currentWaypointIndex++;
            HandleWaypointReached();
        }
        
    }
    
    private bool HasReachedTarget()
    {
        return Vector3.Distance(transform.position, targetPosition) < 1f;
    }
    private void HandleWaypointReached()
    {
        if (checkable)
        {
            rotateable = true;
            checkable = false;
            if (currentWaypointIndex <= CarWayPoint.WayPoints.Count - 1)
            {
                PerformWaypointTransition(CarWayPoint.WayPoints[currentWaypointIndex].position);
            }
            else
            {
                Initiliaze(CarWayPoint);
            }
        }
        else
        {
            ResetSpeedIfNecessary();
        }
    }
    private void PerformWaypointTransition(Vector3 nextWaypointPosition)
    {
        DOTween.To(() => speed, x => speed = x, 2f, 1.2f);
        transform.DOLookAt(nextWaypointPosition, 1.2f).OnComplete(SetSpeed);
    }

    private void ResetSpeedIfNecessary()
    {
        if (speed != 5f)
        {
            speed = 5f;
            rotateable = false;
        }
    }

    public void MoveToTarget()
    {
        if (!rotateable)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            transform.LookAt(targetPosition);
        }
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
    public void SetSpeed()
    {
        speed = 5f;
        checkable = true;
        rotateable = false;
    }
    bool IsNear()
    {
        // Işıkla araç arasındaki mesafeyi kontrol et
        return Vector3.Distance(transform.position, CarWayPoint.TrafficLights[0].transform.position) < 5f;
    }
}

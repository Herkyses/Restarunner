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
    private const float maxSpeed = 5f;
    private const float wheelRotationDuration = 0.2f;
    private const float slowDownDuration = 0.5f;
    private const float raycastDistance = 4f;
    private const float stopDistanceFromLight = 5f;
    
    public int currentWaypointIndex = 0;
    public bool checkable = true;
    public bool rotateable = false;
    public bool startedRotation = false;
    public Vector3 targetPosition ;
    public Transform[] RaycastTransforms ;
    public bool stopped = false;
    [SerializeField] private Transform _raycastsParent;
    public LayerMask ignoreLayer;
    // Start is called before the first frame update
    public void Initiliaze(WayPoint wayPoint)
    {
        ResetCarState();
        //StartWheelRotation();
        CarWayPoint = wayPoint;
        currentWaypointIndex = 0;
        transform.position = wayPoint.WayPoints[0].transform.position;
        transform.rotation = Quaternion.Euler(transform.forward);
    }

    private void Start()
    {
        RaycastTransforms = _raycastsParent.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsObjectInFrontOf(Player.Instance.gameObject)) return;

        if (IsNearTrafficLight() && IsRedLight())
        {
            StopCar();
        }
        else
        {
            ToggleWheelRotation(true);
            MoveToNextWaypoint();
        }
        
    }

    
    private void StartWheelRotation()
    {
        if (startedRotation) return;
        
        startedRotation = true;
        KillWheelTweens();
        AdjustSpeed(maxSpeed,0.8f);
        foreach (var wheel in _wheels)
        {
            var tween = wheel.DOLocalRotate(new Vector3(wheel.eulerAngles.x + 360f, 0, 0), wheelRotationDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1);
            _wheelTweens.Add(tween);
        }
    }
    private void KillWheelTweens()
    {
        foreach (var tween in _wheelTweens)
        {
            tween.Kill();
        }
        _wheelTweens.Clear();
    }
    
    private void StopWheelRotation()
    {
        if (!startedRotation) return;

        startedRotation = false;
        foreach (var tween in _wheelTweens)
        {
            tween.Pause();
        }

        _wheelTweens.Clear();
    }
    private void MoveToNextWaypoint()
    {
        if (currentWaypointIndex >= CarWayPoint.WayPoints.Count) return;

        targetPosition = CarWayPoint.WayPoints[currentWaypointIndex].position;
        
        
        MoveToTarget();
        
        if (HasReachedTarget())
        {
            currentWaypointIndex++;
            if (currentWaypointIndex <= CarWayPoint.WayPoints.Count - 1)
            {
                HandleWaypointReached();
            }
            else
            {
                Initiliaze(CarWayPoint);
            }
        }
        
    }
    private bool IsNearTrafficLight()
    {
        return Vector3.Distance(transform.position, CarWayPoint.TrafficLights[0].transform.position) < stopDistanceFromLight;
    }
    private bool IsRedLight()
    {
        return CarWayPoint.TrafficLights[0].currentState == TrafficLight.LightState.Red;
    }

    private void StopCar()
    {
        ToggleWheelRotation(false);
        AdjustSpeed(0f,slowDownDuration);

    }
    private bool HasReachedTarget()
    {
        return Vector3.Distance(transform.position, targetPosition) < 1f;
    }
   
    private void HandleWaypointReached()
    {
        if (checkable)
        {
            StartWaypointTransition(CarWayPoint.WayPoints[currentWaypointIndex].position);
        }
        else
        {
            ResetSpeedIfNecessary();
        }
    }

    private void StartWaypointTransition(Vector3 nextWaypointPosition)
    {
        checkable = false;
        rotateable = true;
        AdjustSpeed(3f,0.6f);
        transform.DOLookAt(nextWaypointPosition, 0.6f).OnComplete(ResetAfterRotation);
    }
    private void ResetAfterRotation()
    {
        checkable = true;
        rotateable = false;
        speed = maxSpeed;
        transform.DOLookAt(targetPosition, 0.2f);
    }
    private void PerformWaypointTransition(Vector3 nextWaypointPosition)
    {
        AdjustSpeed(3f,0.6f);
        transform.DOLookAt(nextWaypointPosition, 0.6f).OnComplete(SetSpeed);
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
            //transform.DOLookAt(targetPosition,0.2f);
        }
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
    
    public void SetSpeed()
    {
        checkable = true;
        rotateable = false;
        speed = 5f;
        transform.DOLookAt(targetPosition,0.2f);
    }
    bool IsNear()
    {
        // Işıkla araç arasındaki mesafeyi kontrol et
        return Vector3.Distance(transform.position, CarWayPoint.TrafficLights[0].transform.position) < 5f;
    }
    
    
    private bool PerformRaycast(Transform raycastTransform, GameObject targetObject)
    {
        Ray ray = new Ray(raycastTransform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance,~ignoreLayer) && (hit.collider.gameObject && hit.collider.gameObject != gameObject ))
        {
            //Debug.DrawRay(ray.origin, ray.direction * 4f, Color.magenta);
            StopCarIfNeeded();
            return true;
        }
        //Debug.DrawRay(ray.origin, ray.direction * 4f, Color.magenta);

        return false;
    }
    private bool IsObjectInFrontOf(GameObject objA)
    {
        foreach (var raycastTransform in RaycastTransforms)
        {
            if (PerformRaycast(raycastTransform, objA))
            {
                return true;
            }
        }

        ResumeMovementIfStopped();
        return false;
    }
    private void AdjustSpeed(float targetSpeed, float duration)
    {
        DOTween.To(() => speed, x => speed = x, targetSpeed, duration);
    }
    private void ResumeMovementIfStopped()
    {
        if (stopped)
        {
            stopped = false;
            AdjustSpeed(maxSpeed,0.8f);
        }
    }

    private void StopCarIfNeeded()
    {
        if (!stopped)
        {
            stopped = true;
            AdjustSpeed(0f,slowDownDuration);
            ToggleWheelRotation(false);
        }
    }
    private void ToggleWheelRotation(bool shouldRotate)
    {
        if (shouldRotate)
        {
            StartWheelRotation();
        }
        else
        {
            StopWheelRotation();
        }
    }
    
    private void ResetCarState()
    {
        speed = maxSpeed;
        checkable = true;
        rotateable = false;
        startedRotation = false;
        stopped = false;
    }
}

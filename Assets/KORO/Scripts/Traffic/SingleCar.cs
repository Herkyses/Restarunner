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
    public Transform[] RaycastTransforms ;
    public bool stopped = false;
    [SerializeField] private Transform _raycastsParent;
    // Start is called before the first frame update
    public void Initiliaze(WayPoint wayPoint)
    {
        speed = 5f;
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
        RaycastTransforms = _raycastsParent.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsObjectInFrontOf(Player.Instance.gameObject, gameObject))
        {
            return;
        }
        
        if ((CarWayPoint.TrafficLights[0] && CarWayPoint.TrafficLights[0].currentState == TrafficLight.LightState.Red && IsNear())  )
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
            for (int i = 0; i < _wheelTweens.Count; i++)
            {
                _wheelTweens[i].Pause();
                _wheelTweens[i].Kill();
            }
            _wheelTweens.Clear();
            
            for (int i = 0; i < _wheels.Count; i++)
            {
                _wheelTweens.Add(_wheels[i].DOLocalRotate(new Vector3(_wheels[i].eulerAngles.x + 360f, 0, 0), 0.2f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1));
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
                    _wheels[i].DOLocalRotate(new Vector3(_wheels[i].eulerAngles.x + 180, 0, 0), 0.5f, RotateMode.FastBeyond360);
                    //_wheelTweens[i].Kill();
                }
                _wheelTweens.Clear();
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
        DOTween.To(() => speed, x => speed = x, 3f, 0.6f);
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
    bool IsObjectInFrontOf(GameObject objA, GameObject objB)
    {
        for (int i = 0; i < RaycastTransforms.Length; i++)
        {
            Ray ray = new Ray(RaycastTransforms[i].position, objB.transform.forward);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * 4f, Color.magenta);

            if (Physics.Raycast(ray, out hit, 4f))
            {
                if (hit.collider.gameObject == objA)
                {
                    Debug.Log("ObjA tam önünde!");
                    if (!stopped)
                    {
                        stopped = true;
                        DOTween.To(() => speed, x => speed = x, 0f, 0.5f);
                        StopWheelRotation();
                    }

                    if (speed != 0)
                    {
                        transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    }
                    return true;
                }
            }
        }
        
        
        if (stopped)
        {
            DOTween.To(() => speed, x => speed = x, 5f, 0.8f);
            startedRotation = false;
            stopped = false;
        }
        

        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAIPedestrianController : MonoBehaviour
{
    [SerializeField] private List<Transform> _wayPoints;
    [SerializeField] private Transform _target;
    
    

    public void Initiliaze(List<Transform> wayPointTransforms)
    {
        _wayPoints = wayPointTransforms;
    }
}

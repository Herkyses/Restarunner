using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class SingleCar : MonoBehaviour
{
    [SerializeField] private List<Transform> _wheels;
    [SerializeField] private List<Tween> _wheelTweens = new List<Tween>();
    // Start is called before the first frame update
    public void Initiliaze()
    {
        StartWheelRotation();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StopWheelRotation();
        }
    }

    public void StartWheelRotation()
    {
        _wheelTweens.Clear();
        for (int i = 0; i < _wheels.Count; i++)
        {
            _wheelTweens.Add(_wheels[i].DOLocalRotate(new Vector3(360f, 0, 0), 0.3f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1));
        }
        TrafficManager.Instance.SetDestination(GetComponent<NavMeshAgent>());
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
}

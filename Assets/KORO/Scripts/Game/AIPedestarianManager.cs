using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPedestarianManager : MonoBehaviour
{
    public static AIPedestarianManager Instance;

    
    [SerializeField] private List<Transform> _padestrianParents;

    private int pedestrianCount;
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
        for (int i = 0; i < _padestrianParents.Count; i++)
        {
            var pedestr = PoolManager.Instance.GetFromPoolForPedestrianAI();
            pedestr.transform.position = _padestrianParents[i].GetChild(0).position;
        }
    }
}

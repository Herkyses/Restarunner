using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            //var pedestr = PoolManager.Instance.GetFromPoolForPedestrianAI();
            var transformList = _padestrianParents[i].GetComponentsInChildren<Transform>().ToList();
            transformList.Remove(_padestrianParents[i]);
            //pedestr.GetComponent<SingleAIPedestrianController>().SetTargetDestination(_padestrianParents[i].GetChild(1));
            for (int j = 0; j < transformList.Count; j++)
            {
                var pedestr = PoolManager.Instance.GetFromPoolForPedestrianAI();
                pedestr.GetComponent<SingleAIPedestrianController>().Initiliaze(transformList,j);

            }
        }
    }

    public IEnumerator SpawnAIWithDelay()
    {
        yield return new WaitForSeconds(2f);
    }
}

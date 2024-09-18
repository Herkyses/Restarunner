using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopularityManager : MonoBehaviour
{

    public static PopularityManager Instance;
    public PopularityData PopularityData;
    private float averagePopularity;
    
    

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


    public void CalculatePopularityRatio(float newCustomerPopularity)
    {
        var customerCount = PlayerPrefsManager.Instance.LoadCustomerCount();
        averagePopularity = ((averagePopularity * (customerCount - 1)) + newCustomerPopularity) / customerCount;
    }
}

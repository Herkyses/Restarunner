using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopularityManager : MonoBehaviour
{

    public static PopularityManager Instance;
    public PopularityData PopularityData;
    private float averagePopularity;
    private float cleanliness;
    private float decorationQuality;
    private float _rate = 10f;
    

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

    public void CalculateSinglePopularityValue(float customerPatienceRate,float serviceSpeed,float tableQuality)
    {
        float customerPopularity = ((serviceSpeed/_rate) * PopularityData.FoodQualityMultiplier) 
                                   + ((cleanliness) * PopularityData.CleanlinessMultiplier)
                                   + ((tableQuality/_rate) * PopularityData.TableQualityMultiplier)
                                   + ((decorationQuality/_rate) * PopularityData.DecorationQualityMultiplier)
                                   + ((customerPatienceRate/_rate) * PopularityData.CustomerPatienceMultiplier);
        
        // Yeni popülariteyi ortalama ile birleştir
        //totalPopularity = ((totalPopularity * (totalCustomers - 1)) + customerPopularity) / totalCustomers;

    }

    public void CalculatePopularityRatio(float newCustomerPopularity)
    {
        var customerCount = PlayerPrefsManager.Instance.LoadCustomerCount();
        averagePopularity = ((averagePopularity * (customerCount - 1)) + newCustomerPopularity) / customerCount;
    }
}

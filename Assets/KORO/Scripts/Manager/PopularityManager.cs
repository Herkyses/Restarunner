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
    private RubbishManager _rubbishManager;
    

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
        _rubbishManager = RubbishManager.Instance;
        GameSceneCanvas.Instance.UpdatePopularity(PlayerPrefsManager.Instance.LoadPopularity());
    }

    public void CalculateSinglePopularityValue(float customerPatienceRate,float serviceSpeed,float tableQuality)
    {
        float customerPopularity = ((serviceSpeed/_rate) * PopularityData.FoodQualityMultiplier) 
                                   + ((1-_rubbishManager.GetCleanRateValue()) * PopularityData.CleanlinessMultiplier)
                                   + ((tableQuality/_rate) * PopularityData.TableQualityMultiplier)
                                   + ((decorationQuality/_rate) * PopularityData.DecorationQualityMultiplier)
                                   + ((customerPatienceRate/_rate) * PopularityData.CustomerPatienceMultiplier);
        
        // Yeni popülariteyi ortalama ile birleştir
        // totalPopularity = ((totalPopularity * (totalCustomers - 1)) + customerPopularity) / totalCustomers;
        Debug.Log("servicespeed" + (serviceSpeed/_rate) * PopularityData.FoodQualityMultiplier);
        Debug.Log("clean" + (_rubbishManager.GetCleanRateValue()) * PopularityData.CleanlinessMultiplier);
        Debug.Log("table" + (tableQuality/_rate) * PopularityData.TableQualityMultiplier);
        Debug.Log("decorationQuality" + (decorationQuality/_rate) * PopularityData.DecorationQualityMultiplier);
        Debug.Log("customerPatienceRate" + (customerPatienceRate/_rate) * PopularityData.CustomerPatienceMultiplier);
        CalculatePopularityRatio(customerPopularity);
    }

    public void CalculatePopularityRatio(float newCustomerPopularity)
    {
        var customerCount = PlayerPrefsManager.Instance.LoadCustomerCount();
        
        if (customerCount > 1)
        {
            averagePopularity = PlayerPrefsManager.Instance.LoadPopularity();
            if (customerCount < 10)
            {
                averagePopularity = ((averagePopularity * (customerCount - 1)) + newCustomerPopularity) / customerCount;
            }
            else
            {
                averagePopularity = (averagePopularity * 0.9f) + (newCustomerPopularity * 0.1f);
            }
        }
        else
        {
            averagePopularity = newCustomerPopularity;
        }
        PlayerPrefsManager.Instance.SavePopularity(averagePopularity);

        GameSceneCanvas.Instance.UpdatePopularity(averagePopularity);
        Debug.Log("pop " + newCustomerPopularity);
    }
}

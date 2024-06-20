using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public static PlayerPrefsManager Instance;
    public static Action<float> GainedMoney;
    public static Action<int> GainedPopularity;
    /*private void OnEnable()
    {
        AIStateMachineController.PayedOrderBill += SavePlayerMoney;
    }

    private void OnDisable()
    {
        AIStateMachineController.PayedOrderBill -= SavePlayerMoney;
    }*/

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
    
    
    public void SavePlayerMoney(float money)
    {
        PlayerPrefs.SetFloat("PlayerMoney", money);
        PlayerPrefs.Save();
    }
    public float LoadPlayerMoney()
    {
        return PlayerPrefs.GetFloat("PlayerMoney", 0); // Varsayılan değer 0
    }
    public void SavePopularity(int money)
    {
        PlayerPrefs.SetInt("Popularity", money);
        PlayerPrefs.Save();
    }
    public int LoadPopularity()
    {
        return PlayerPrefs.GetInt("Popularity", 0); // Varsayılan değer 0
    }
    public void SaveMeals(MealsList mealsList)
    {
        string jsonData = JsonUtility.ToJson(mealsList);
        PlayerPrefs.SetString("MealsData", jsonData);
        PlayerPrefs.Save();
    }

    public MealsList LoadMeals()
    {
        if (PlayerPrefs.HasKey("MealsData"))
        {
            string jsonData = PlayerPrefs.GetString("MealsData");
            MealsList mealsList = JsonUtility.FromJson<MealsList>(jsonData);
            return mealsList;
        }
        return null;
    }
}

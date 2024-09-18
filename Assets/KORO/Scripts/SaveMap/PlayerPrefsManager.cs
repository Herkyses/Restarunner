using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public static PlayerPrefsManager Instance;
    public static Action<float> GainedMoney;
    public static Action<int> GainedPopularity;

    public static Action TutorialStepUpdated;
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
    public void SavePlayerPlayerTutorialStep(int step)
    {
        PlayerPrefs.SetInt("PlayerTutorialStep", step);
        PlayerPrefs.Save();
        //TutorialStepUpdated?.Invoke();
    }
    public int LoadDecorationCount()
    {
        return PlayerPrefs.GetInt("DecorationCount", 0); 
    }
    public void SaveDecorationCount(int decorationCount)
    {
        PlayerPrefs.SetInt("DecorationCount", decorationCount);
        PlayerPrefs.Save();
        //TutorialStepUpdated?.Invoke();
    }
    public int LoadPlayerTutorialStep()
    {
        return PlayerPrefs.GetInt("PlayerTutorialStep", 0); 
    }
    
    public void SavePlayerMoney(float money)
    {
        PlayerPrefs.SetFloat("PlayerMoney", money);
        PlayerPrefs.Save();
    }
    public float LoadPlayerMoney()
    {
        return PlayerPrefs.GetFloat("PlayerMoney", 25); 
    }
    public void SavePlaceLevel(int level)
    {
        PlayerPrefs.SetInt("PlaceLevel", level);
        PlayerPrefs.Save();
    }
    public int LoadPlaceLevel()
    {
        return PlayerPrefs.GetInt("PlaceLevel", 0); 
    }
    public void SaveCustomerCount(int customerCount)
    {
        PlayerPrefs.SetInt("CustomerCount", customerCount);
        PlayerPrefs.Save();
    }
    public int LoadCustomerCount()
    {
        return PlayerPrefs.GetInt("CustomerCount", 0); 
    }
    public void SaveMeals(MealsList mealsList)
    {
        string jsonData = JsonUtility.ToJson(mealsList);
        PlayerPrefs.SetString("MealsData", jsonData);
        PlayerPrefs.Save();
    }
    public void SaveMealIngredients(MealIngredientsList mealIngredientsList)
    {
        string jsonData = JsonUtility.ToJson(mealIngredientsList);
        PlayerPrefs.SetString("MealIngredientsData", jsonData);
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
    public MealIngredientsList LoadMealIngredients()
    {
        if (PlayerPrefs.HasKey("MealIngredientsData"))
        {
            string jsonData = PlayerPrefs.GetString("MealIngredientsData");
            MealIngredientsList mealsList = JsonUtility.FromJson<MealIngredientsList>(jsonData);
            return mealsList;
        }
        return null;
    }
    
    public void SavePlaceRubbishLevel(int level)
    {
        PlayerPrefs.SetInt("PlaceRubbishLevel", level);
        PlayerPrefs.Save();
    }
    public int LoadPlaceRubbishLevel()
    {
        return PlayerPrefs.GetInt("PlaceRubbishLevel", 0);
    }
    public void SavePopularity(float popularity)
    {
        PlayerPrefs.SetFloat("Popularity", popularity);
        PlayerPrefs.Save();
    }
    public float LoadPopularity()
    {
        return PlayerPrefs.GetFloat("Popularity", 0);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public static PlayerPrefsManager Instance;
    public static Action<float,float> GainedMoney;
    public static Action<int> GainedPopularity;

    public static Action TutorialStepUpdated;
    
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
    private void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    private int LoadInt(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    private void SaveFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    private float LoadFloat(string key, float defaultValue = 0f)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    private void SaveString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    private string LoadString(string key, string defaultValue = "")
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }
    
    // TUTORIAL
    public void SavePlayerTutorialStep(int step) => SaveInt("PlayerTutorialStep", step);
    public int LoadPlayerTutorialStep() => LoadInt("PlayerTutorialStep");
    public void SaveDecorationCount(int decorationCount) => SaveInt("DecorationCount", decorationCount);
    public int LoadDecorationCount() => LoadInt("DecorationCount");
    // MONEY
    public void SavePlayerMoney(float money) => SaveFloat("PlayerMoney", money);
    public float LoadPlayerMoney() => LoadFloat("PlayerMoney", 50);
    
    // PLACELEVEL
    public void SavePlaceLevel(int level) => SaveInt("PlaceLevel", level);
    public int LoadPlaceLevel() => LoadInt("PlaceLevel");
    
    // CUSTOMERCOUNT
    public void SaveCustomerCount(int customerCount) => SaveInt("CustomerCount", customerCount);
    public int LoadCustomerCount() => LoadInt("CustomerCount");
    
    // PLACELEVEL
    public void SaveResolution(int index) => SaveInt("Resolution", index);
    public int LoadResolution() => LoadInt("Resolution");
    
    // LANGUAGE
    public void SaveLanguage(int index) => SaveInt("Language", index);
    public int LoadLanguage() => LoadInt("Language");
    
    // QUALITY
    public void SaveQuality(int index) => SaveInt("Quality", index);
    public int LoadQuality() => LoadInt("Quality");
    
    
    // SFX
    public void SaveVolume(float money) => SaveFloat("sfxVolume", money);
    public float LoadVolume() => LoadFloat("sfxVolume", 50);
    public void SaveMealIngredients(MealIngredientsList mealIngredientsList)
    {
        string jsonData = JsonUtility.ToJson(mealIngredientsList);
        SaveString("MealIngredientsData", jsonData);
    }

    public MealIngredientsList LoadMealIngredients()
    {
        string jsonData = LoadString("MealIngredientsData");
        return !string.IsNullOrEmpty(jsonData) ? JsonUtility.FromJson<MealIngredientsList>(jsonData) : null;
    }

    public MealsList LoadMeals()
    {
        string jsonData = LoadString("MealsData");
        return !string.IsNullOrEmpty(jsonData) ? JsonUtility.FromJson<MealsList>(jsonData) : null;
    }
    
    public void SavePlaceRubbishLevel(int level) => SaveInt("PlaceRubbishLevel", level);
    public int LoadPlaceRubbishLevel() => LoadInt("PlaceRubbishLevel");
    public void SavePopularity(float popularity) => SaveFloat("Popularity", popularity);
    public float LoadPopularity() => LoadFloat("Popularity");
    
    
}

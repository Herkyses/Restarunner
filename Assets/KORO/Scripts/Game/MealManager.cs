using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MealManager : MonoBehaviour
{
    public static MealManager Instance;
    public static Action UpdateFoodIngredient;
    
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
    private SaveManager saveManager;
    private MealsList mealsList;

    public void Initiliaze()
    {
        saveManager = new SaveManager();
        LoadMeals();

        // Test: Burger yapma işlemi
        //MakeMeal("Burger");
    }

    public void LoadMeals()
    {
        mealsList = PlayerPrefsManager.Instance.LoadMeals();
        if (mealsList == null)
        {
            // Eğer veriler yoksa, başlangıç verilerini yükle
            List<Meal> meals = new List<Meal>
            {
                new Meal(Enums.OrderType.Burger, 5),
                new Meal(Enums.OrderType.Pizza, 5),
                new Meal(Enums.OrderType.Salad, 5),
                new Meal(Enums.OrderType.Pasta, 5),
                new Meal(Enums.OrderType.Soup, 5),
                new Meal(Enums.OrderType.Sandwich, 5),
                new Meal(Enums.OrderType.Omelette, 5)
            };

            mealsList = new MealsList(meals);
            PlayerPrefsManager.Instance.SaveMeals(mealsList);
        }
    }

    public void MakeMeal(Enums.OrderType mealName,int value)
    {
        Meal meal = mealsList.meals.Find(m => m.mealName == mealName);
        if (meal != null && meal.ingredientQuantity > 0)
        {
            meal.ingredientQuantity += value;
            PlayerPrefsManager.Instance.SaveMeals(mealsList);
            Debug.Log("Made " + mealName + ". Remaining quantity: " + meal.ingredientQuantity);
        }
        else
        {
            Debug.Log("Cannot make " + mealName + ". Not enough ingredients.");
        }
        UpdateFoodIngredient?.Invoke();
    }
    
}

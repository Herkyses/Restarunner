using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MealManager : MonoBehaviour
{
    public static MealManager Instance;
    
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

    private void Start()
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
                new Meal("Burger", 5),
                new Meal("Pizza", 5),
                new Meal("Salad", 5),
                new Meal("Pasta", 5),
                new Meal("Soup", 5),
                new Meal("Sandwich", 5),
                new Meal("Omelette", 5)
            };

            mealsList = new MealsList(meals);
            PlayerPrefsManager.Instance.SaveMeals(mealsList);
        }
    }

    public void MakeMeal(string mealName)
    {
        Meal meal = mealsList.meals.Find(m => m.mealName == mealName);
        if (meal != null && meal.ingredientQuantity > 0)
        {
            meal.ingredientQuantity--;
            PlayerPrefsManager.Instance.SaveMeals(mealsList);
            Debug.Log("Made " + mealName + ". Remaining quantity: " + meal.ingredientQuantity);
        }
        else
        {
            Debug.Log("Cannot make " + mealName + ". Not enough ingredients.");
        }
    }
    
}

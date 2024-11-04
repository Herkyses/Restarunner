using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MealManager : MonoBehaviour
{
    public static MealManager Instance;
    public static Action UpdateFoodIngredient;
    
    private SaveManager saveManager;
    private MealsList mealsList;
    [SerializeField]private MealIngredientsList mealIngredientsList;
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
        saveManager = new SaveManager();
        LoadMealsSecondWay();

        // Test: Burger yapma işlemi
        //MakeMeal("Burger");
    }

    
    public void LoadMealsSecondWay()
    {
        
        mealIngredientsList = PlayerPrefsManager.Instance.LoadMealIngredients();
        if (mealIngredientsList == null)
        {
            // Eğer veriler yoksa, başlangıç verilerini yükle
            List<MealIngredient> meals = new List<MealIngredient>
            {
                
                new MealIngredient(Enums.FoodIngredientType.Bread, 5),
                new MealIngredient(Enums.FoodIngredientType.Cheese, 5),
                new MealIngredient(Enums.FoodIngredientType.Eggs, 5),
                new MealIngredient(Enums.FoodIngredientType.Flour, 5),
                new MealIngredient(Enums.FoodIngredientType.Lettuce, 5),
                new MealIngredient(Enums.FoodIngredientType.Meat, 5),
                new MealIngredient(Enums.FoodIngredientType.Milk, 5),
                new MealIngredient(Enums.FoodIngredientType.Onion, 5),
                new MealIngredient(Enums.FoodIngredientType.Tomato, 5),
                new MealIngredient(Enums.FoodIngredientType.ChickenBreast, 5),
                new MealIngredient(Enums.FoodIngredientType.OtherVegetables, 5),
            };

            mealIngredientsList = new MealIngredientsList(meals);
            PlayerPrefsManager.Instance.SaveMealIngredients(mealIngredientsList);
        }
    }

    public void MakeMeal(Enums.OrderType mealName,int value)
    {
        Meal meal = mealsList.meals.Find(m => m.mealName == mealName);
        if (meal != null )
        {
            if (value < 0 && meal.ingredientQuantity > 0)
            {
                meal.ingredientQuantity += value;
            }
            else if(value > 0)
            {
                meal.ingredientQuantity += value;
            }
            PlayerPrefsManager.Instance.SaveMeals(mealsList);
            Debug.Log("Made " + mealName + ". Remaining quantity: " + meal.ingredientQuantity);
        }
        else
        {
            Debug.Log("Cannot make " + mealName + ". Not enough ingredients.");
        }
        UpdateFoodIngredient?.Invoke();
    }
    public void MakeMealIngredient(Enums.OrderType orderType,int value)
    {
        var foodData = GameDataManager.Instance.GetFoodData(orderType);
        bool ingredientsUpdated = false;
        for (int i = 0; i < foodData.FoodIngredientTypes.Count; i++)
        {
            MealIngredient meal = mealIngredientsList.meals.Find(m => m.mealName == foodData.FoodIngredientTypes[i]);
            if (meal != null )
            {
                if (value < 0 && meal.ingredientQuantity > 0)
                {
                    meal.ingredientQuantity += value;
                }
                else if(value > 0)
                {
                    meal.ingredientQuantity += value;
                }
                ingredientsUpdated = true;
                Debug.Log("Made " + foodData.FoodIngredientTypes[i] + ". Remaining quantity: " + meal.ingredientQuantity);
            }
            else
            {
                Debug.Log("Cannot make " + foodData.FoodIngredientTypes[i] + ". Not enough ingredients.");
            }
        }

        if (ingredientsUpdated)
        {
            PlayerPrefsManager.Instance.SaveMealIngredients(mealIngredientsList);
        }
        
    }
    public void MakeSingleMealIngredient(Enums.FoodIngredientType orderType,int value)
    {
        //var foodData = GameDataManager.Instance.GetFoodData(orderType);
        MealIngredient meal = mealIngredientsList.meals.Find(m => m.mealName == orderType);
        if (meal != null )
        {
            if (value < 0 && meal.ingredientQuantity > 0)
            {
                meal.ingredientQuantity += value;
            }
            else if(value > 0)
            {
                meal.ingredientQuantity += value;
            }
            PlayerPrefsManager.Instance.SaveMealIngredients(mealIngredientsList);
            //Debug.Log("Made " + foodData.FoodIngredientTypes[i] + ". Remaining quantity: " + meal.ingredientQuantity);
        }
        else
        {
            //Debug.Log("Cannot make " + foodData.FoodIngredientTypes[i] + ". Not enough ingredients.");
        }
    }
    
    public int GetFoodIngredient(Enums.FoodIngredientType mealName)
    {
        var mealsList = PlayerPrefsManager.Instance.LoadMealIngredients();

        MealIngredient meal = mealsList.meals.Find(m => m.mealName == mealName);
        if (meal != null && meal.ingredientQuantity > 0)
        {
            return meal.ingredientQuantity;
            
        }
        else
        {
            return 0;
        }
    }
    
}

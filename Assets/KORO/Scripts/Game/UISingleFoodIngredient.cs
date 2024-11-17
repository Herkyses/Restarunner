using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISingleFoodIngredient : MonoBehaviour
{
    
    [SerializeField] private Image _uiIngredientImage;
    [SerializeField] private TextMeshProUGUI _uiIngredientText;
    [SerializeField] private Enums.FoodIngredientType _uiIngredientType;

    private void OnEnable()
    {
        ChefController.OnFoodIngredientDecreased += UpdateIngredient;
        ChefController.OnFoodIngredientUpdated += UpdateIngredientInfo;
    }
    private void OnDisable()
    {
        ChefController.OnFoodIngredientDecreased -= UpdateIngredient;
        ChefController.OnFoodIngredientUpdated -= UpdateIngredientInfo;
    }
    public void Initiliaze(Enums.FoodIngredientType ingredientType)
    {
        _uiIngredientType = ingredientType;
        UpdateIngredientInfo();
    }

    public void UpdateIngredient(OrderData orderData)
    {
        UpdateIngredientInfo();
    }

    public void UpdateIngredientInfo()
    {
        _uiIngredientImage.sprite = GameDataManager.Instance.GetFoodIngredientIcon(_uiIngredientType);
        _uiIngredientText.text = MealManager.Instance.GetFoodIngredient(_uiIngredientType).ToString();
    }
}

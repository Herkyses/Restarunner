using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISingleFood : MonoBehaviour
{
    private OrderData _uiOrderData;
    [SerializeField] private Image _uiOrderImage;
    [SerializeField] private UISingleFoodIngredient _uiSingleFoodIngredientPf;

    public void Initiliaze(OrderData orderData)
    {
        _uiOrderData = orderData;
        _uiOrderImage.sprite = orderData.FoodIcon;
        Utilities.DeleteTransformchilds(gameObject.transform);
        CreateIngredients();
    }

    private void CreateIngredients()
    {
        var types = _uiOrderData.FoodIngredientTypes;
        for (int i = 0; i < types.Count; i++)
        {
            var ingredient = Instantiate(_uiSingleFoodIngredientPf, transform);
            ingredient.Initiliaze(GameDataManager.Instance.GetFoodIngredientIcon(types[i]));
        }
    }
}

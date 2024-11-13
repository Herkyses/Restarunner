using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISingleFoodIngredient : MonoBehaviour
{
    
    [SerializeField] private Image _uiIngredientImage;

    public void Initiliaze(Sprite uiSprite)
    {
        _uiIngredientImage.sprite = uiSprite;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SingleGredientShelves : MonoBehaviour,IInterectableObject
{
    public Enums.FoodIngredientType shelveIngredientType;
    [SerializeField] private List<Transform> _ingredientTransformList;
    [SerializeField] private List<GameObject> _ingredientList;
    [SerializeField] private int _count;


    private void OnEnable()
    {
        ChefController.FoodIngredientIncreese += CheckIngredients;
    }
    private void OnDisable()
    {
        ChefController.FoodIngredientIncreese -= CheckIngredients;
    }

    public void CheckIngredients(OrderData orderData)
    {
        var ingredientHere = false;
        for (int i = 0; i < orderData.FoodIngredientTypes.Count; i++)
        {
            if (orderData.FoodIngredientTypes[i] == shelveIngredientType)
            {
                IngredientCount();
                return;
            }
        }
        
        
        
        
        
    }

    public void IngredientCount()
    {
        var firstCount = _count;
        var mealManager = MealManager.Instance;
        _count = mealManager.GetFoodIngredient(shelveIngredientType);
        for (int i = 0; i < firstCount; i++)
        {
            if (i >= _count)
            {
                PoolManager.Instance.ReturnToPoolForFoodIngredient(_ingredientList[i]);
            }
            
        }
    }

    public void InterectableObjectRun()
    {
        var takedObject = Player.Instance.PlayerTakedObject;
        if (takedObject)
        {
            if (takedObject.GetComponent<SingleCrate>().GetIngredientType() == shelveIngredientType)
            {
                MealManager.Instance.MakeSingleMealIngredient(shelveIngredientType,1);
                var singleIngredient = Player.Instance.PlayerTakedObject.GetComponent<SingleCrate>().GetIngredientObject();
                _ingredientList.Add(singleIngredient);
                singleIngredient.transform.SetParent(transform);
                singleIngredient.transform.DOLocalMove(singleIngredient.transform.localPosition + Vector3.up*0.2f, 0.15f).OnComplete(() =>
                {
                    singleIngredient.transform.DOMove(_ingredientTransformList[_count].position, 0.2f);
                    singleIngredient.transform.DORotate(_ingredientTransformList[_count].rotation.eulerAngles, 0.2f);
                    /*singleIngredient.transform.position = _ingredientTransformList[_count].position;
                    singleIngredient.transform.rotation = _ingredientTransformList[_count].rotation;*/
                    _count++;
                });            
                
            }
        }
        
    }

    public void Initiliaze()
    {
        for (int i = 0; i < MealManager.Instance.GetFoodIngredient(shelveIngredientType); i++)
        {
            var singleIngredient = PoolManager.Instance.GetFromPoolForFoodIngredient(); //getfrom pool 
            _ingredientList.Add(singleIngredient);
            singleIngredient.transform.SetParent(transform);
            singleIngredient.transform.position = _ingredientTransformList[i].position;
            singleIngredient.transform.rotation = _ingredientTransformList[i].rotation;
            singleIngredient.transform.localScale = Vector3.one;
            singleIngredient.GetComponent<MeshFilter>().sharedMesh = GameDataManager.Instance.GetFoodIngredientMeshFilter(shelveIngredientType).sharedMesh;
            singleIngredient.GetComponent<SingleIngredient>().IngredientType = shelveIngredientType;
            //singleIngredient.Get
            _count++;
        }
    }
    public void ShowOutline(bool active)
    {
        
    }

    public Outline GetOutlineComponent()
    {
        return null;

    }
    public string GetInterectableText()
    {
        return null;

    }
    public string[] GetInterectableTexts()
    {
        return null;

    }
    public string[] GetInterectableButtons()
    {
        return null;
    }
    public void Move()
    {
    
    }
    public void Open()
    {
    
    }
    public Enums.PlayerStateType GetStateType()
    {
        return Enums.PlayerStateType.TakeFoodIngredient;
    }
}

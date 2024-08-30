using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleGredientShelves : MonoBehaviour,IInterectableObject
{
    public Enums.FoodIngredientType shelveIngredientType;
    [SerializeField] private List<Transform> _ingredientTransformList;
    [SerializeField] private int _count;

    public void InterectableObjectRun()
    {
        var takedObject = Player.Instance.PlayerTakedObject;
        if (takedObject)
        {
            if (takedObject.GetComponent<SingleCrate>().GetIngredientType() == shelveIngredientType)
            {
                MealManager.Instance.MakeSingleMealIngredient(shelveIngredientType,1);
                var singleIngredient = Player.Instance.PlayerTakedObject.GetComponent<SingleCrate>().GetIngredientObject();
                singleIngredient.transform.SetParent(transform);
                singleIngredient.transform.position = _ingredientTransformList[_count].position;
                _count++;
            }
        }
        
    }

    public void Initiliaze()
    {
        Debug.Log("aynen öylee bee" + MealManager.Instance.GetFoodIngredient(shelveIngredientType));
        for (int i = 0; i < MealManager.Instance.GetFoodIngredient(shelveIngredientType); i++)
        {
            var singleIngredient = PoolManager.Instance.GetFromPoolForFoodIngredient(); //getfrom pool 
            singleIngredient.transform.SetParent(transform);
            singleIngredient.transform.position = _ingredientTransformList[i].position;
            singleIngredient.GetComponent<MeshFilter>().sharedMesh = GameDataManager.Instance.GetFoodIngredientMeshFilter(shelveIngredientType).sharedMesh;
            singleIngredient.GetComponent<SingleIngredient>().IngredientType = shelveIngredientType;
            //singleIngredient.Get
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

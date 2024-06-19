using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTable : MonoBehaviour,IInterectableObject
{
    public Enums.OrderType OrderType;
    public GameObject FoodObject;
    public Food Food;
    public Transform FoodSpawnTransform;
    public void InterectableObjectRun()
    {
        PlayerOrderController.Instance.TakeFood(GetComponent<FoodTable>());
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
        return "Take Food";
    }
    public void Move()
    {
        
    }

    public void CreateFood(Enums.OrderType orderType)
    {
        DeleteChilds(FoodSpawnTransform);
        var food = Instantiate(GameDataManager.Instance.GetFood(orderType),FoodSpawnTransform);
        Food = food;
        food.transform.position = Vector3.zero;
    }
    
    public void DeleteChilds(Transform parent)
    {
        var orderArray = parent.GetComponentsInChildren<FoodTable>();
        if (orderArray.Length > 0)
        {
            for (int i = 0; i < orderArray.Length; i++)
            {
                Destroy(orderArray[i].gameObject);
            }
        }
    }
}

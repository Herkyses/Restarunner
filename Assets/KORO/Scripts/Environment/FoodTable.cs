using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTable : MonoBehaviour,IInterectableObject
{
    [SerializeField] private string[] texts = new [] {"Take OrderBox"};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    public Enums.OrderType OrderType;
    public GameObject FoodObject;
    public Food Food;
    public bool IsFoodFinished;
    public Transform FoodSpawnTransform;
    public WaiterController OwnerWaiterCotroller;

    private void Start()
    {
        texts = new [] {"Take Food"};
        textsButtons = new [] {"E"};
    }

    public void InterectableObjectRun()
    {
        PlayerOrderController.Instance.TakeFood(GetComponent<FoodTable>(),IsFoodFinished);
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
    public void Open()
    {
        
    }
    public string[] GetInterectableTexts()
    {
        return texts;
    }

    public void CreateFood(Enums.OrderType orderType)
    {
        DeleteChilds(FoodSpawnTransform);
        //var food = Instantiate(GameDataManager.Instance.GetFood(orderType),FoodSpawnTransform);
        var food = PoolManager.Instance.GetFromPoolForFood().GetComponent<Food>();
        food.transform.SetParent(FoodSpawnTransform);
        food.transform.position = Vector3.zero;
        food.OrderType = orderType;
        food.GetComponent<MeshFilter>().sharedMesh = GameDataManager.Instance.GetFood(orderType).GetComponent<MeshFilter>().sharedMesh;
        food.transform.localPosition = Vector3.zero;
        Food = food;
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
    public string[] GetInterectableButtons()
    {
        return textsButtons;
    }
}

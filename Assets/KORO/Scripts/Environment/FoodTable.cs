using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodTable : MonoBehaviour,IInterectableObject
{
    [SerializeField] private string[] texts = new [] {"Take OrderBox"};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    [SerializeField] private Image _foodQualityImage;
    [SerializeField] private GameObject _foodQualityCanvasObject;
    private Outline _outline;
    
    public Enums.OrderType OrderType;
    public GameObject FoodObject;
    public Food Food;
    public bool IsFoodFinished;
    public bool QualityTimeStarted;
    public bool IsFoodServiced;
    
    public Transform FoodSpawnTransform;
    public float WaitTime;
    public int WaitTimeTempValue;
    public int currentWaitTimeInt;

    public WaiterController OwnerWaiterCotroller;

    private void Start()
    {
        WaitTime = 10f;
        texts = new [] {"Key_PickUp_Food"};
        textsButtons = new [] {"E"};
        _outline = GetComponent<Outline>();
    }

    public void InterectableObjectRun()
    {
        if (IsFoodServiced)
        {
            return;
        }
        PlayerOrderController.Instance.TakeFood(GetComponent<FoodTable>(),IsFoodFinished);
        ChefController.Instance.IsAvailableFoodTable();
    }

    public void ShowOutline(bool active)
    {
        if (IsFoodServiced)
        {
            return;
        }
        _outline.enabled = active;
    }

    public Outline GetOutlineComponent()
    {
        if (!IsFoodServiced)
        {
            return _outline;
        }

        return null;
    }

    public string GetInterectableText()
    {
        if (IsFoodServiced)
        {
            return null;
        }
        return "Key_PickUp_Food";
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
        Utilities.DeleteTransformchilds(FoodSpawnTransform);
        //var food = Instantiate(GameDataManager.Instance.GetFood(orderType),FoodSpawnTransform);
        var food = PoolManager.Instance.GetFromPoolForFood().GetComponent<Food>();
        food.transform.SetParent(FoodSpawnTransform);
        food.transform.position = Vector3.zero;
        food.transform.localRotation = Quaternion.Euler(Vector3.zero);
        food.OrderType = orderType;
        food.GetComponent<MeshFilter>().sharedMesh = GameDataManager.Instance.GetFood(orderType).GetComponent<MeshFilter>().sharedMesh;
        food.transform.localPosition = Vector3.zero;
        Food = food;
        _foodQualityCanvasObject.SetActive(true);
        WaitTime = food.QualityWaitTime;
        IsFoodFinished = false;
        IsFoodServiced = false;
        QualityTimeStarted = true;
    }

    public float GetWaitFoodTime()
    {
        return WaitTime;
    }

    public void FoodGivedCustomer()
    {
        _foodQualityCanvasObject.SetActive(false);
        QualityTimeStarted = false;
        IsFoodServiced = true;
    }
    
    

    public void EatedFood()
    {
        PoolManager.Instance.ReturnToPoolForFood(Food.gameObject);
    }
    public string[] GetInterectableButtons()
    {
        return textsButtons;
    }
    public Enums.PlayerStateType GetStateType()
    {
        return Enums.PlayerStateType.Free;
    }
    void Update()
    {
        if (!QualityTimeStarted || WaitTime <= 0) return;

        WaitTime -= Time.deltaTime;

        currentWaitTimeInt = (int)WaitTime;
        if (WaitTimeTempValue != currentWaitTimeInt)
        {
            _foodQualityImage.fillAmount = WaitTime / 10f;
            WaitTimeTempValue = currentWaitTimeInt;
        }

        if (WaitTime <= 0)
        {
            QualityTimeStarted = false;
        }
    }
}

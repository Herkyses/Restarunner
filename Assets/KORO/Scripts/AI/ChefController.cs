using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefController : MonoBehaviour,IInterectableObject
{
    [SerializeField] private List<OrderDataStruct> ChefOwnerStructData;
    [SerializeField] private List<OrderDataStruct> RemovedOrderDataStructs;
    [SerializeField] private Transform FoodParent;
    [SerializeField] private ChefOrderTable _chefOrderTable;
    [SerializeField] private int _chefOrderTableIndex;
    public static Action<WaiterController> FoodCreated;
    private FoodTable _tableFood;
    private List<FoodTable> _tableFoodList = new List<FoodTable>();

    private void OnEnable()
    {
        GiveChefOrderPanelController.IsGivedToChef += SetOrders;
    }

    private void OnDisable()
    {
        GiveChefOrderPanelController.IsGivedToChef -= SetOrders;
    }

    public void SetOrders(List<OrderDataStruct> orderDataStruct,bool isPlayerGive = true,WaiterController ownerWaiter = null)
    {
        ChefOwnerStructData = orderDataStruct;
        if (!isPlayerGive)
        {
            CreateFoods(true);
            ownerWaiter.FoodTable = _tableFoodList;
            FoodCreated?.Invoke(ownerWaiter);
        }
        else
        {
            CreateFoods(false);

        }
        //orderDataStruct.Clear();        ////////////önemliiiiii yapılanları çıkar sadece
    }

    public void CreateFoods(bool withWaiter)
    {
        for (int i = 0; i < ChefOwnerStructData.Count; i++)
        {
            for (int j = 0; j < GameDataManager.Instance.FoodDatas.Count; j++)
            {
                if (ChefOwnerStructData[i].OrderType == GameDataManager.Instance.FoodDatas[j].OrderType && MealManager.Instance.GetMealIngredient(GameDataManager.Instance.FoodDatas[j].OrderType) > 0)
                {
                    //var food = Instantiate(GameDataManager.Instance.FoodTablePf);
                    var food = PoolManager.Instance.GetFromPool().GetComponent<FoodTable>();
                    food.IsFoodFinished = false;
                    RemovedOrderDataStructs.Add(ChefOwnerStructData[i]);
                    food.CreateFood(GameDataManager.Instance.FoodDatas[j].Food.OrderType);
                    food.transform.position = _chefOrderTable.FoodTransformList[_chefOrderTableIndex].position;
                    food.transform.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
                    if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() > 4)
                    {
                        PlacePanelController.Instance.DecreeseIngredient(food.OrderType);
                        MealManager.Instance.MakeMeal(GameDataManager.Instance.FoodDatas[j].Food.OrderType,-1);
                    }
                    else
                    {
                        TutorialManager.Instance.SetTutorialInfo(8);
                    }
                    _chefOrderTableIndex++;
                    _tableFoodList.Add(food);
                    if (_chefOrderTableIndex >= _chefOrderTable.FoodTransformList.Count)
                    {
                        _chefOrderTableIndex = 0;
                    }
                }
            }

            
        }
        for (int j = 0; j < RemovedOrderDataStructs.Count; j++)
        {
            ChefOwnerStructData.Remove(RemovedOrderDataStructs[j]);
        }
        RemovedOrderDataStructs.Clear();
    }
    public void InterectableObjectRun()
    {
        
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
        return "GiveOrder";
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
        return Enums.PlayerStateType.Free;
    }
}

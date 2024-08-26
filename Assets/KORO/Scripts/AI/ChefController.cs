using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefController : MonoBehaviour,IInterectableObject
{
    [SerializeField] private List<OrderDataStruct> ChefOwnerStructData;
    [SerializeField] private List<OrderDataStruct> RemovedOrderDataStructs;
    
    [SerializeField] private string[] texts = new [] {"Give Order "};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    private Outline _chefOutline;

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

    private void Start()
    {
        texts = new []{"Give Order "};
        textsButtons = new []{"E"};
        _chefOutline = GetComponent<Outline>();

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
                var foodData = GameDataManager.Instance.FoodDatas[j];
                if (ChefOwnerStructData[i].OrderType == foodData.OrderType && MealManager.Instance.GetMealIngredient(foodData.OrderType) > 0)
                {
                    //var food = Instantiate(GameDataManager.Instance.FoodTablePf);
                    var food = PoolManager.Instance.GetFromPoolForFoodTable().GetComponent<FoodTable>();
                    food.IsFoodFinished = false;
                    food.QualityTimeStarted = false;
                    RemovedOrderDataStructs.Add(ChefOwnerStructData[i]);
                    food.CreateFood(foodData.Food.OrderType);
                    food.transform.position = _chefOrderTable.FoodTransformList[_chefOrderTableIndex].position;
                    food.transform.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
                    //MealManager.Instance.MakeMealIngredient(foodData.Food.OrderType,-1);

                    if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() > 4)
                    {
                        PlacePanelController.Instance.DecreeseIngredient(food.OrderType);
                        MealManager.Instance.MakeMeal(foodData.Food.OrderType,-1);
                        //MealManager.Instance.MakeMealIngredient(foodData.Food.OrderType,-1);
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
        var orderPanel = GiveChefOrderPanelController.Instance;
        orderPanel.Panel.gameObject.SetActive(true);
        GameSceneCanvas.Instance.CanMove = false;
        orderPanel.OrderList = PlayerOrderController.Instance.OrderList;
        orderPanel.SelectedOrderListCount = 0;
        orderPanel.OrderListIndexIncrease(true);
    }

    public void ShowOutline(bool active)
    {
        _chefOutline.enabled = active;
    }

    public Outline GetOutlineComponent()
    {
        return _chefOutline;
    }

    public string GetInterectableText()
    {
        return "GiveOrder";
    }
    public string[] GetInterectableTexts()
    {
        return texts;
    }
    public string[] GetInterectableButtons()
    {
        return textsButtons;
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

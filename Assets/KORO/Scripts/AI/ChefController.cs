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
    public static Action<OrderData> FoodIngredientIncreese;
    private FoodTable _tableFood;
    private List<FoodTable> _tableFoodList = new List<FoodTable>();
    
    private const float FOOD_ROTATION_Y = 90f;
    private const int TUTORIAL_STEP_THRESHOLD = 4;
    private const int TUTORIAL_UPDATE_STEP = 8;

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
        var foodDatas = GameDataManager.Instance.FoodDatas;
        for (int i = 0; i < ChefOwnerStructData.Count; i++)
        {
            for (int j = 0; j < foodDatas.Count; j++)
            {
                var foodData = foodDatas[j];
                if (!CheckFoodIngredientCount(foodData))
                {
                    continue;
                }

                if (ChefOwnerStructData[i].OrderType == foodData.OrderType && MealManager.Instance.GetMealIngredient(foodData.OrderType) > 0)
                {
                    
                    var food = InitiliazeFood(foodData);
                    RemovedOrderDataStructs.Add(ChefOwnerStructData[i]);
                    
                    HandleMealIngredient(foodData);
                    TutorialSte(foodData);
                    UpdateChefOrderTableIndex(food);
                }
            }

            
        }
        for (int j = 0; j < RemovedOrderDataStructs.Count; j++)
        {
            ChefOwnerStructData.Remove(RemovedOrderDataStructs[j]);
        }
        RemovedOrderDataStructs.Clear();
    }

    public FoodTable InitiliazeFood(OrderData orderData)
    {
        var food = PoolManager.Instance.GetFromPoolForFoodTable().GetComponent<FoodTable>();
        food.IsFoodFinished = false;
        food.QualityTimeStarted = false;
        food.CreateFood(orderData.Food.OrderType);
        food.transform.position = _chefOrderTable.FoodTransformList[_chefOrderTableIndex].position;
        food.transform.localRotation = Quaternion.Euler(new Vector3(0f, FOOD_ROTATION_Y, 0f));
        return food;
    }
    
    public void HandleMealIngredient(OrderData orderData)
    {
        MealManager.Instance.MakeMealIngredient(orderData.Food.OrderType,-1);
        FoodIngredientIncreese?.Invoke(orderData);
    }

    public void TutorialSte(OrderData orderData)
    {
        if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() > TUTORIAL_STEP_THRESHOLD)
        {
            //PlacePanelController.Instance.DecreeseIngredient(orderData.OrderType);
        }
        else
        {
            TutorialManager.Instance.SetTutorialInfo(TUTORIAL_UPDATE_STEP);
        }
    }

    public bool CheckFoodIngredientCount(OrderData foodOrderData)
    {

        for (int i = 0; i < foodOrderData.FoodIngredientTypes.Count; i++)
        {
            var ingredientCount = MealManager.Instance.GetFoodIngredient(foodOrderData.FoodIngredientTypes[i]);
            if (ingredientCount > 0)
            {
                return true;
            }
        }
        return false;
    }

    public void UpdateChefOrderTableIndex(FoodTable foodTable)
    {
        _chefOrderTableIndex++;
        _tableFoodList.Add(foodTable);
        if (_chefOrderTableIndex >= _chefOrderTable.FoodTransformList.Count)
        {
            _chefOrderTableIndex = 0;
        }
    }
    public void InterectableObjectRun()
    {
        GiveChefOrderPanelController.Instance.OrderPanelInitliaze();
        
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

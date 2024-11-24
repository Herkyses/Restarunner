using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ChefController : MonoBehaviour,IInterectableObject
{
    public static ChefController Instance;
    [Header("Order Data")]
    [SerializeField] private List<OrderDataStruct> chefOrderData = new List<OrderDataStruct>();
    [SerializeField] private List<OrderDataStruct> removedOrderData = new List<OrderDataStruct>();
    [SerializeField] private OrderDataStruct currentFoodData;
    
    [Header("UI Elements")]
    [SerializeField] private string[] interactionTexts = new[] { "Key_Give_Order_Chef" };
    [SerializeField] private string[] interactionButtons = new[] { "E" };
    [SerializeField] private Image chefCreateFoodProgressIcon;
    
    
    [Header("Chef Properties")]
    [SerializeField] private Transform FoodParent;
    [SerializeField] private ChefOrderTable _chefOrderTable;
    [SerializeField] private float foodCreationDuration;
    [SerializeField] private bool isCreatingFood;
    [SerializeField] private int orderTableIndex;
    
    [Header("References")]
    private Outline _chefOutline;
    
    public static Action<WaiterController> OnFoodCreated;
    public static Action<OrderData> OnFoodIngredientDecreased;
    public static Action OnFoodIngredientUpdated;
    
    private List<FoodTable> createdFoodTables = new List<FoodTable>();
    
    private const float FOOD_ROTATION_Y = 90f;
    private const int TUTORIAL_STEP_THRESHOLD = 4;
    private const int TUTORIAL_UPDATE_STEP = 9;

    private void OnEnable()
    {
        GiveChefOrderPanelController.OnOrderGivenToChef += AddOrders;
    }

    private void OnDisable()
    {
        GiveChefOrderPanelController.OnOrderGivenToChef -= AddOrders;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        _chefOutline = GetComponent<Outline>();
        chefCreateFoodProgressIcon.fillAmount = 0f;
        chefCreateFoodProgressIcon.gameObject.SetActive(false);
    }
    
    
    
    
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.T))
        {
            SimulateOrder();
        }
#endif
        
        
        if (isCreatingFood && chefOrderData.Count > 0 && IsAvailableFoodTable() && CheckIngredient())
        {
            ProcessFoodCreation();
        }
        else if (chefCreateFoodProgressIcon.fillAmount != 0f)
        {
            ResetFoodCreation();
        }
        
    }
    private void SimulateOrder()
    {
        var sampleOrder = new OrderDataStruct { OrderPrice = 5, OrderType = Enums.OrderType.Sandwich };
        AddOrders(new List<OrderDataStruct> { sampleOrder, sampleOrder });
    }
    private void ProcessFoodCreation()
    {
        if (chefCreateFoodProgressIcon.sprite == null)
        {
            InitializeFoodCreation();
        }

        if (!chefCreateFoodProgressIcon.gameObject.activeSelf)
        {
            chefCreateFoodProgressIcon.gameObject.SetActive(true);
        }
        foodCreationDuration += Time.deltaTime;
        UpdateFoodCreationProgress();

        if (foodCreationDuration > 5f)
        {
            CreateFood();
            foodCreationDuration = 0f;
            chefCreateFoodProgressIcon.sprite = null;
            chefCreateFoodProgressIcon.gameObject.SetActive(false);
            chefOrderData.Remove(currentFoodData);
        }
    }
    private void InitializeFoodCreation()
    {
        transform.DOLocalRotate(new Vector3(0, FOOD_ROTATION_Y, 0), 0.2f);
        currentFoodData = chefOrderData[0];
        chefCreateFoodProgressIcon.sprite = GameDataManager.Instance.GetFoodSprite(currentFoodData.OrderType);
    }
    private void UpdateFoodCreationProgress()
    {
        chefCreateFoodProgressIcon.fillAmount = foodCreationDuration / 5f;
    }
    private void ResetFoodCreation()
    {
        chefCreateFoodProgressIcon.fillAmount = 0f;
        transform.localRotation = Quaternion.Euler(Vector3.up * -90f);
        chefCreateFoodProgressIcon.sprite = null;
        isCreatingFood = false;
    }
    public void AddOrders(List<OrderDataStruct> orders, bool isPlayerInitiated = true, WaiterController waiter = null)
    {
        chefOrderData.AddRange(orders);
        isCreatingFood = true;

        if (!isPlayerInitiated)
        {
            CreateAllFoods();
            waiter.FoodTable = createdFoodTables;
            OnFoodCreated?.Invoke(waiter);
        }
    }
    private void CreateAllFoods()
    {
        foreach (var order in chefOrderData)
        {
            foreach (var foodData in GameDataManager.Instance.FoodDatas)
            {
                if (order.OrderType == foodData.OrderType  && CheckFoodIngredientCount(foodData))
                {
                    var food = InitializeFood(foodData);
                    removedOrderData.Add(order);
                    DecreaseIngredients(foodData);
                    HandleTutorialSteps(foodData);
                    UpdateFoodTableIndex(food);
                }
            }
        }
        ResetOrders();
    }
    private void CreateFood()
    {
        foreach (var foodData in GameDataManager.Instance.FoodDatas)
        {
            if (currentFoodData.OrderType == foodData.OrderType && CheckFoodIngredientCount(foodData))
            {
                var food = InitializeFood(foodData);
                removedOrderData.Add(currentFoodData);
                DecreaseIngredients(foodData);
                HandleTutorialSteps(foodData);
                UpdateFoodTableIndex(food);
            }
        }
    }

    private bool CheckIngredient()
    {
        currentFoodData = chefOrderData[0];
        foreach (var foodData in GameDataManager.Instance.FoodDatas)
        {
            if (currentFoodData.OrderType == foodData.OrderType && CheckFoodIngredientCount(foodData))
            {
                return true;
            }
        }

        isCreatingFood = false;
        return false;
    }
    public bool IsAvailableFoodTable()
    {
        orderTableIndex = -1;

        for (int i = 0; i < _chefOrderTable.FoodTransformList.Count; i++)
        {
            if (_chefOrderTable.FoodTransformList[i].childCount == 0)
            {
                orderTableIndex = i;
                isCreatingFood = true;
                return true;
            }
        }

        isCreatingFood = false;
        return false;
    }
    private FoodTable InitializeFood(OrderData foodData)
    {
        var foodTable = PoolManager.Instance.GetFromPoolForFoodTable().GetComponent<FoodTable>();
        foodTable.CreateFood(foodData.Food.OrderType);
        foodTable.transform.SetParent(_chefOrderTable.FoodTransformList[orderTableIndex]);
        foodTable.transform.localPosition = Vector3.zero;
        foodTable.transform.localRotation = Quaternion.Euler(Vector3.up * FOOD_ROTATION_Y);
        return foodTable;
    }
    
    private void DecreaseIngredients(OrderData foodData)
    {
        MealManager.Instance.MakeMealIngredient(foodData.Food.OrderType, -1);
        OnFoodIngredientDecreased?.Invoke(foodData);
    }
    private void HandleTutorialSteps(OrderData foodData)
    {
        if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() > TUTORIAL_STEP_THRESHOLD)
        {
            // Additional tutorial logic
        }
        else
        {
            TutorialManager.Instance.SetTutorialInfo(TUTORIAL_UPDATE_STEP);
        }
    }
    
    public bool CheckFoodIngredientCount(OrderData foodOrderData)
    {
        var mealManager = MealManager.Instance;
        for (int i = 0; i < foodOrderData.FoodIngredientTypes.Count; i++)
        {
            var ingredientCount = mealManager.GetFoodIngredient(foodOrderData.FoodIngredientTypes[i]);
            if (ingredientCount == 0)
            {
                return false;
            }
        }
        return true;
    }
    private void UpdateFoodTableIndex(FoodTable foodTable)
    {
        createdFoodTables.Add(foodTable);
        orderTableIndex = (orderTableIndex + 1) % _chefOrderTable.FoodTransformList.Count;
    }
    
    
    private void ResetOrders()
    {
        foreach (var removedOrder in removedOrderData)
        {
            chefOrderData.Remove(removedOrder);
        }
        removedOrderData.Clear();
    }

    public List<OrderDataStruct> GetOrderData()
    {
        return chefOrderData;
    }

    public bool GetIsCreating()
    {
        return isCreatingFood;
    }
    public void SetIsCreating(bool creating)
    {
        isCreatingFood = creating;
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
        return "Key_Give_Order_Chef";
    }
    public string[] GetInterectableTexts()
    {
        return interactionTexts;
    }
    public string[] GetInterectableButtons()
    {
        return interactionButtons;
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

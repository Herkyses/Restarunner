using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ChefController : MonoBehaviour,IInterectableObject
{
    public static ChefController Instance;
    [SerializeField] private List<OrderDataStruct> ChefOwnerStructData;
    [SerializeField] private List<OrderDataStruct> RemovedOrderDataStructs;
    [SerializeField] private OrderDataStruct CurrentFoodData;
    
    [SerializeField] private string[] texts = new [] {"Give Order "};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    private Outline _chefOutline;

    [SerializeField] private Transform FoodParent;
    [SerializeField] private ChefOrderTable _chefOrderTable;
    [SerializeField] private int _chefOrderTableIndex;
    [SerializeField] private bool _chefCreateFood;
    [SerializeField] private float _chefCreateFoodDuring;
    [SerializeField] private Image _chefCreateFoodDuringIcon;
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
        texts = new []{"Give Order "};
        textsButtons = new []{"E"};
        _chefOutline = GetComponent<Outline>();
        _chefCreateFoodDuringIcon.fillAmount = 0f;

    }
    
    
    
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.T))
        {
            var zort = new OrderDataStruct()
            {
                OrderPrice =  5,
                OrderType = Enums.OrderType.Sandwich
            };
            var listo = new List<OrderDataStruct>();
            listo.Add(zort);
            listo.Add(zort);
            SetOrders(listo);
        }
#endif
        if (_chefCreateFood)
        {
            if (IsAvailableFoodTable())
            {
                StartFoodDuring();
            }
        }
    }

    private void StartFoodDuring()
    {
        if (!_chefCreateFoodDuringIcon.sprite)
        {
            transform.DOLocalRotate(new Vector3(0,90f,0f),0.2f);
            CurrentFoodData = ChefOwnerStructData[0];
            _chefCreateFoodDuringIcon.sprite = GameDataManager.Instance.GetFoodSprite(CurrentFoodData.OrderType);
        }
        if (ChefOwnerStructData.Count > 0)
        {
            _chefCreateFoodDuring += Time.deltaTime;
            CreateFoodFill();
            if (_chefCreateFoodDuring > 5)
            {
                CurrentFoodData = ChefOwnerStructData[0];
                CreateFood();
                ChefOwnerStructData.Remove(CurrentFoodData);
                _chefCreateFoodDuring = 0;
            }
        }
        else
        {
            if (_chefCreateFoodDuringIcon.fillAmount != 0f)
            {
                _chefCreateFoodDuringIcon.fillAmount = 0f;
                transform.localRotation = Quaternion.Euler(new Vector3(0,-90f,0f));
                _chefCreateFoodDuringIcon.sprite = null;
                _chefCreateFood = false;
            }

        }
    }

    private void CreateFoodFill()
    {
        _chefCreateFoodDuringIcon.fillAmount = _chefCreateFoodDuring / 5f;
    }

    public void SetOrders(List<OrderDataStruct> orderDataStruct,bool isPlayerGive = true,WaiterController ownerWaiter = null)
    {
        //ChefOwnerStructData = orderDataStruct;
        for (int i = 0; i < orderDataStruct.Count; i++)
        {
            ChefOwnerStructData.Add(orderDataStruct[i]);
        }

        _chefCreateFood = true;
        
        
        if (!isPlayerGive)
        {
            CreateFoods(true);
            ownerWaiter.FoodTable = _tableFoodList;
            FoodCreated?.Invoke(ownerWaiter);
        }
        else
        {
            //CreateFoods(false);
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
                
                if (ChefOwnerStructData[i].OrderType == foodDatas[j].OrderType && MealManager.Instance.GetMealIngredient(foodDatas[j].OrderType) > 0)
                {
                    if (!CheckFoodIngredientCount(foodDatas[j]))
                    {
                        continue;
                    }
                    var food = InitiliazeFood(foodDatas[j]);
                    RemovedOrderDataStructs.Add(ChefOwnerStructData[i]);
                    
                    HandleMealIngredient(foodDatas[j]);
                    TutorialSte(foodDatas[j]);
                    UpdateChefOrderTableIndex(food);
                }
            }

            
        }
        //ResetChefOwner();
    }

    public void CreateFood()
    {
        var foodDatas = GameDataManager.Instance.FoodDatas;
        
        for (int j = 0; j < foodDatas.Count; j++)
        {
                
            if (CurrentFoodData.OrderType == foodDatas[j].OrderType && MealManager.Instance.GetMealIngredient(foodDatas[j].OrderType) > 0)
            {
                if (!CheckFoodIngredientCount(foodDatas[j]))
                {
                    continue;
                }
                var food = InitiliazeFood(foodDatas[j]);
                RemovedOrderDataStructs.Add(CurrentFoodData);
                    
                HandleMealIngredient(foodDatas[j]);
                TutorialSte(foodDatas[j]);
                UpdateChefOrderTableIndex(food);
            }
        }

            
        
    }

    public bool IsAvailableFoodTable()
    {
        _chefOrderTableIndex = -1;
        for (int i = 0; i < _chefOrderTable.FoodTransformList.Count; i++)
        {
            if (_chefOrderTable.FoodTransformList[i].childCount == 0)
            {
                _chefOrderTableIndex = i;
                _chefCreateFood = true;
                return true;
            }
        }

        _chefCreateFood = false;
        return false;
    }

    private void ResetChefOwner()
    {
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
        food.transform.SetParent(_chefOrderTable.FoodTransformList[_chefOrderTableIndex]);
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

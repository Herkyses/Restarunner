using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private PlayerRaycastController _playerRaycastController;
    public PlayerOrderController PlayerOrdersController;
    public bool _canTakeMoney;
    public bool CanCleanRubbish;
    public bool CanFight;
    public float PlayerMoney;
    [SerializeField] private GameObject[] PlayerInventory;
    public GameObject PlayerTakedObject;
    public Enums.PlayerStateType PlayerStateType;
    public PlayerStructData PlayerStructData;
    // Start is called before the first frame update
    
    
    public static Player Instance;
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
    private void OnEnable()
    {
        GameManager.PayedOrderBill += GainMoney;
    }

    private void OnDisable()
    {
        GameManager.PayedOrderBill -= GainMoney;
    }
    
    void Start()
    {
        gameObject.TryGetComponent(out _playerRaycastController);
        gameObject.TryGetComponent(out PlayerOrdersController);
        PlayerMoney = PlayerPrefsManager.Instance.LoadPlayerMoney();
        CameraController.Instance.PlayerTransform = transform;
    }

    public void ActivatedRaycast(bool active)
    {
        _playerRaycastController.IsRaycastActive = active;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            //CanCleanRubbish = !CanCleanRubbish;
            CameraController.Instance.StateInitiliazeForTakeObject(Enums.PlayerStateType.Cleaner);

        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            //CanFight = !CanFight;
            CameraController.Instance.StateInitiliazeForTakeObject(Enums.PlayerStateType.Fight);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() > 0)
            {
                ControllerManager.Instance.PlacePanelController.OpenPlacePanel();
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (PlayerTakedObject && PlayerTakedObject.TryGetComponent(out OrderBox orderBox))
            {
                PlayerTakedObject = null;
                orderBox.Open();
            }
        }
    }

    public void DropTakenObject()
    {
        PlayerTakedObject = null;
    }

    public void GainMoney(float gainValue)
    {
        /*if (_canTakeMoney)
        {
            //_playerData.OwnedMoney += gainValue;
            UpdatePopularity(1);
            PlayerStructData.Money += gainValue*(PlayInputController.Instance.score/100)*PlayInputController.Instance.CorrectAnswerCount;
            //GameSceneCanvas.Instance.UpdateMoneyText(PlayerStructData.Money,PlayerStructData.Popularity);
        }*/
        PlayerPrefsManager.GainedMoney?.Invoke(gainValue,PlayerMoney);
        PlayerMoney += gainValue;
        PlayerPrefsManager.Instance.SavePlayerMoney(PlayerMoney);
        //PlayerPrefsManager.GainedMoney?.Invoke(PlayerMoney);

    }

    private void UpdatePopularity(int popularity)
    {
        PlayerStructData.Popularity += popularity;
    }

    public void StartClean()
    {
        CameraController.Instance.MoveCleanTool();
        RubbishManager.Instance.CleanStarted();
    }

    public void StartFight()
    {
        CameraController.Instance.MoveFightTool();
    }

    public void MoveObject(GameObject moveObject,Enums.PlayerStateType playerStateType)
    {
        ActivatedRaycast(false);
        PlayerTakedObject = moveObject;
        PlayerStateType = playerStateType;
    }

    public void FreePlayerStart()
    {
        PlayerOrdersController.TakedTableBill = false;
        TakedObjectNull();
    }

    public void TakedObjectNull()
    {
        PlayerTakedObject = null;
        PlayerStateType = Enums.PlayerStateType.Free;
    }

    public void TakedObject(GameObject takeObject,Enums.PlayerStateType stateType)
    {
        PlayerTakedObject = takeObject;
        PlayerStateType = stateType;
    }
    
}
[Serializable]
public struct PlayerStructData
{
    public float Money;
    public int Popularity;
    
}

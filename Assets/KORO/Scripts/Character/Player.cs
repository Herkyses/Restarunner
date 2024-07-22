using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    public bool _canTakeMoney;
    public bool CanCleanRubbish;
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
        PlayerMoney = PlayerPrefsManager.Instance.LoadPlayerMoney();
        CameraController.Instance.PlayerTransform = transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CanCleanRubbish = !CanCleanRubbish;
            CameraController.Instance.CleanToolActive(CanCleanRubbish);
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
        PlayerMoney += gainValue;
        PlayerPrefsManager.Instance.SavePlayerMoney(PlayerMoney);
        PlayerPrefsManager.GainedMoney?.Invoke(PlayerMoney);

    }

    private void UpdatePopularity(int popularity)
    {
        PlayerStructData.Popularity += popularity;
    }
    
}
[Serializable]
public struct PlayerStructData
{
    public float Money;
    public int Popularity;
    
}

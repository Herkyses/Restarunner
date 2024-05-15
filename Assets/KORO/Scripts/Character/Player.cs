using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    public bool _canTakeMoney;
    [SerializeField] private GameObject[] PlayerInventory;
    
    public PlayerStructData PlayerStructData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GainMoney(12f);
        }
    }

    public void GainMoney(float gainValue)
    {
        if (_canTakeMoney)
        {
            //_playerData.OwnedMoney += gainValue;
            UpdatePopularity(1);
            PlayerStructData.Money += gainValue*(PlayInputController.Instance.score/100)*PlayInputController.Instance.CorrectAnswerCount;
            GameSceneCanvas.Instance.UpdateMoneyText(PlayerStructData.Money,PlayerStructData.Popularity);
        }
        
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

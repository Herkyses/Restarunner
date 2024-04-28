using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;

    
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
        //_playerData.OwnedMoney += gainValue;
        PlayerStructData.Money += gainValue;
        GameSceneCanvas.Instance.UpdateMoneyText(PlayerStructData.Money);
    }
    
}
[Serializable]
public struct PlayerStructData
{
    public float Money;
}

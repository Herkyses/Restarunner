using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public static PlayerPrefsManager Instance;
    public static Action<float> GainedMoney;
    /*private void OnEnable()
    {
        AIStateMachineController.PayedOrderBill += SavePlayerMoney;
    }

    private void OnDisable()
    {
        AIStateMachineController.PayedOrderBill -= SavePlayerMoney;
    }*/

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

    public void PlayerMoney(float money)
    {
        
        SavePlayerMoney(money);
    }
    
    public void SavePlayerMoney(float money)
    {
        PlayerPrefs.SetFloat("PlayerMoney", money);
        PlayerPrefs.Save();
    }
    public float LoadPlayerMoney()
    {
        return PlayerPrefs.GetFloat("PlayerMoney", 0); // Varsayılan değer 0
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player PlayerPrefab;
    public Transform PlayerSpawnTransform;
    public static Action<float> PayedOrderBill; 


    public static Action GameStarted;
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
        GameStarted?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreatePlayer()
    {
        var player = Instantiate(PlayerPrefab);
        player.transform.position = PlayerSpawnTransform.position;
    }
}

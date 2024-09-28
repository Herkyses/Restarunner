using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeManager : MonoBehaviour
{
    public static InitializeManager Instance;
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
        GameManager.GameStarted += Initialize;
    }

    private void OnDisable()
    {
        GameManager.GameStarted -= Initialize;
    }

    // Start is called before the first frame update
    public void Initialize()
    {

        MapManager.Instance.LoadMap();
        TrafficManager.Instance.Initiliaze();

        PoolManager.Instance.Initiliaze();
        MealManager.Instance.Initiliaze();
        TableController.Instance.SetTableNumbers();
        TableController.Instance.Initialize();
        TableAvailablePanel.Instance.Initialize();
        CheckOrderBillsPanel.Instance.Initialize();


        //StartCoroutine(AISpawnController.Instance.Initialize());
        OrderPanelController.Instance.Initialize();
        PlacePanelController.Instance.Initialize();
        PlaceController.Instance.Initialize();
        RubbishManager.Instance.Initiliaze();
        IngredientShelvesController.Instance.Initiliaze();
        PopularityManager.Instance.Initiliaze();
        TutorialManager.Instance.Initiliaze();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

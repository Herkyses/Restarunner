using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InitializeManager : MonoBehaviour
{
    public static InitializeManager Instance;
    [Inject] private CheckOrderBillsPanel _checkOrderBillsPanel;
    [Inject] private OrderPanelController _orderPanelController;

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
        var controllerManager = ControllerManager.Instance;
        MapManager.Instance.LoadMap();
        TrafficManager.Instance.Initiliaze();
        AIPedestarianManager.Instance.Initiliaze();
        
        PoolManager.Instance.Initiliaze();
        MealManager.Instance.Initiliaze();
        controllerManager.Tablecontroller.Initialize();
        controllerManager.TableAvailablePanel.Initialize();
        controllerManager._checkOrderBillsPanel.Initialize();


        //StartCoroutine(AISpawnController.Instance.Initialize());
        controllerManager._orderPanelController.Initialize();
        controllerManager.PlacePanelController.Initialize();
        ControllerManager.Instance.PlaceController.Initialize();
        RubbishManager.Instance.Initiliaze();
        IngredientShelvesController.Instance.Initiliaze();
        PopularityManager.Instance.Initiliaze();
        TutorialManager.Instance.Initiliaze();

    }

    
}

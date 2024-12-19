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
    public static GameManager Instance;


    public static Action GameStarted;
    public static Action<bool> IsAnyPanelOpened;
    public static bool IsPanelOpened;


    private void OnEnable()
    {
        IsAnyPanelOpened += PanelOpened;
    }

    private void OnDisable()
    {
        IsAnyPanelOpened -= PanelOpened;
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
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
        GameStarted?.Invoke();
    }

    public void PanelOpened(bool isPanelOpened)
    {
        IsPanelOpened = isPanelOpened;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HandleTablePlacementCompletion()
    {
        MapManager.Instance.SaveMap();
        GameSceneCanvas.Instance.CheckShowInfoText = true;
        ControllerManager.Instance.PlaceController.ActivateDecorationPlane(false);
    }

    public void CreatePlayer()
    {
        var player = Instantiate(PlayerPrefab);
        player.transform.position = PlayerSpawnTransform.position;
    }
    public void CheckAndProgressTutorialStep(int currentStep, int nextStep)
    {
        if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == currentStep)
        {
            PlayerPrefsManager.Instance.SavePlayerTutorialStep(nextStep);
            TutorialManager.Instance.SetTutorialInfo(nextStep);
        }
    }
    
    public void ProcessOrderPayment(float billValue)
    {
        PayedOrderBill?.Invoke(billValue);
    }
}

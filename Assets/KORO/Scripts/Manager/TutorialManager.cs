using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    private int totalStepCount;
    
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
        PlayerPrefsManager.TutorialStepUpdated += Initiliaze;
    }

    private void OnDisable()
    {
        PlayerPrefsManager.TutorialStepUpdated -= Initiliaze;
    }

    private void Start()
    {
        totalStepCount = 5;
    }

    public void Initiliaze()
    {
        totalStepCount = 5;
        SetTutorialInfo(PlayerPrefsManager.Instance.LoadPlayerTutorialStep());
    }

    public void SetTutorialInfo(int step)
    {
        switch (step)
        {
            case 0:
                TutorialPanelController.Instance.ActivateTutorialPanel(true);
                TutorialPanelController.Instance.ActivateRemainingText(true);
                
                TutorialPanelController.Instance.SetTutorialInfoText("Clean Restaurant\n " + "Use a broom\n <color=green>Press P</color>");
                TutorialPanelController.Instance.SetRubbishCount();
                break;
            case 1:
                TutorialPanelController.Instance.ActivateTutorialPanel(true);
                TutorialPanelController.Instance.SetTutorialInfoText("Buy Table Set from Shop");
                break;
            case 2:
                TutorialPanelController.Instance.ActivateTutorialPanel(true);
                TutorialPanelController.Instance.SetTutorialInfoText("Take The Order Box outside and Open in Restaurant");
                break;
            case 3:
                TutorialPanelController.Instance.ActivateTutorialPanel(true);
                TutorialPanelController.Instance.SetTutorialInfoText("Move and Place The Table");
                break;
            case 4:
                TutorialPanelController.Instance.ActivateTutorialPanel(true);
                TutorialPanelController.Instance.SetTutorialInfoText("Open Restaurant and Guide customers to the table");
                break;
            case 5:
                TutorialPanelController.Instance.ActivateTutorialPanel(true);
                TutorialPanelController.Instance.SetTutorialInfoText("Take Customer Order from Table");
                break;
            case 6:
                TutorialPanelController.Instance.ActivateTutorialPanel(true);
                TutorialPanelController.Instance.SetTutorialInfoText("You can add order to your order inventory from foods with plus Button");
                break;
            case 7:
                TutorialPanelController.Instance.ActivateTutorialPanel(true);
                TutorialPanelController.Instance.SetTutorialInfoText("Give order to chef");
                break;
            case 8:
                TutorialPanelController.Instance.ActivateTutorialPanel(true);
                TutorialPanelController.Instance.SetTutorialInfoText("Take and Serv Food to Customer");
                break;
            case 9:
                TutorialPanelController.Instance.ActivateTutorialPanel(true);
                TutorialPanelController.Instance.SetTutorialInfoText("If the customer has finished their meal, select the relevant table from the billing desk and take the bill to the table");
                break;
            default:
                TutorialPanelController.Instance.ActivateTutorialPanel(false);
                break;
            
        }
    }
}

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
                
                TutorialPanelController.Instance.SetTutorialInfoText("To clean the restaurant, pick up the broom. <color=green>Press 'P'</color>  to pick up the broom.");
                TutorialPanelController.Instance.SetRubbishCount();
                break;
            case 1:
                TutorialPanelController.Instance.ActivateTutorialPanel(true);
                TutorialPanelController.Instance.SetTutorialInfoText("Purchase the table set from the store using the computer.");
                break;
            case 2:
                TutorialPanelController.Instance.ActivateTutorialPanel(true);
                TutorialPanelController.Instance.SetTutorialInfoText("Bring the order box from outside into the restaurant and open it.");
                break;
            case 3:
                TutorialPanelController.Instance.ActivateTutorialPanel(true);
                TutorialPanelController.Instance.SetTutorialInfoText("Move and set up the table set.");
                break;
            case 4:
                TutorialPanelController.Instance.ActivateTutorialPanel(true);
                TutorialPanelController.Instance.SetTutorialInfoText("Open the restaurant and guide customers to their tables.");
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
            case 10:
                TutorialPanelController.Instance.ActivateTutorialPanel(false);
                break;
            default:
                TutorialPanelController.Instance.ActivateTutorialPanel(false);
                break;
            
        }
    }
}

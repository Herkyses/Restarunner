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
                TutorialPanelController.Instance.SetTutorialInfoText("Buy table set");
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                TutorialPanelController.Instance.ActivateTutorialPanel(false);
                break;
            
        }
    }
}

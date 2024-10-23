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

    private List<TutorialStep> tutorialSteps;

    

    public void SetTutorialTexts()
    {
        tutorialSteps = new List<TutorialStep>
        {
            new TutorialStep("To clean the restaurant, pick up the broom. <color=green>Press 'C'</color>  to pick up the broom.", true, true, () => TutorialPanelController.Instance.SetRubbishCount()),
            new TutorialStep("Press the <color=green>Press 'I'</color> key and use the tablet to purchase a table set."),
            new TutorialStep("Bring the order box from outside into the restaurant and open it."),
            new TutorialStep("Move and set up the table set."),
            new TutorialStep("Open the restaurant and guide customers from the waiting area to their tables."),
            new TutorialStep("Take orders from the table."),
            new TutorialStep("You can add items to your order inventory by using the plus button next to the food."),
            new TutorialStep("Give the order to the chef."),
            new TutorialStep("Pick up the food and serve it."),
            new TutorialStep("If the customer has finished their meal, select the corresponding table from the billing desk and bring the bill to the table."),
            new TutorialStep("", false)
        };
    }

    public void Initiliaze()
    {
        SetTutorialTexts();
        Debug.Log("tutstep:" +PlayerPrefsManager.Instance.LoadPlayerTutorialStep() );
        SetTutorialInfo(PlayerPrefsManager.Instance.LoadPlayerTutorialStep());
    }

    public void SetTutorialInfo(int step)
    {
        if (step < 0 || step >= tutorialSteps.Count)
        {
            TutorialPanelController.Instance.ActivateTutorialPanel(false);
            return;
        }

        var currentStep = tutorialSteps[step];

        TutorialPanelController.Instance.ActivateTutorialPanel(currentStep.ShowPanel);

        if (currentStep.ShowPanel)
        {
            TutorialPanelController.Instance.SetTutorialInfoText(currentStep.InfoText);
            if (currentStep.ShowRemainingText)
                TutorialPanelController.Instance.ActivateRemainingText(true);

            currentStep.CustomAction?.Invoke();
        }
    }
}
public class TutorialStep
{
    public string InfoText { get; set; }
    public bool ShowPanel { get; set; }
    public bool ShowRemainingText { get; set; }
    public Action CustomAction { get; set; }

    public TutorialStep(string infoText, bool showPanel = true, bool showRemainingText = false, Action customAction = null)
    {
        InfoText = infoText;
        ShowPanel = showPanel;
        ShowRemainingText = showRemainingText;
        CustomAction = customAction;
    }
}

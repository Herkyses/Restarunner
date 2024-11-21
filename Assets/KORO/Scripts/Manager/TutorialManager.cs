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
            new TutorialStep("Key_Tut0", true, true, () => TutorialPanelController.Instance.SetRubbishCount()),
            new TutorialStep("Key_Tut1"),
            new TutorialStep("Key_Tut2"),
            new TutorialStep("Key_Tut3"),
            new TutorialStep("Key_Tut4"),
            new TutorialStep("Key_Tut5"),
            new TutorialStep("Key_Tut6"),
            new TutorialStep("Key_Tut7"),
            new TutorialStep("Key_Tut8"),
            new TutorialStep("Key_Tut9"),
            new TutorialStep("Key_Tut10"),
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

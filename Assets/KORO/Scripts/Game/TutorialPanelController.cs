using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialPanelController : MonoBehaviour
{
    public static TutorialPanelController Instance;

    public Transform TutorialPanel;
    public TextMeshProUGUI TutorialText;
    public TextMeshProUGUI TutorialTextForRemainingRubbish;
    
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

    public void ActivateTutorialPanel(bool active)
    {
        TutorialPanel.gameObject.SetActive(active);
    }

    public void SetTutorialInfoText(string text)
    {
        TutorialText.text = text;
    }

    public void SetRubbishCount()
    {
        TutorialTextForRemainingRubbish.text = RubbishManager.Instance.GetRubbishCount().ToString() + "Remaining";
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class TutorialPanelController : MonoBehaviour
{
    public static TutorialPanelController Instance;

    public Transform TutorialPanel;
    public TextMeshProUGUI TutorialText;
    public TextMeshProUGUI TutorialTextForRemainingRubbish;
    public LocalizeStringEvent localizeStringEvent;
    
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

    public void SetTutorialInfoText(string textKey)
    {
        //TutorialText.text = textKey;
        Debug.Log("textkey:" + textKey);
        localizeStringEvent.StringReference.TableEntryReference = textKey;
        localizeStringEvent.RefreshString();
    }

    public void SetRubbishCount()
    {
        TutorialTextForRemainingRubbish.gameObject.SetActive(true);
        var a = RubbishManager.Instance.GetRubbishCount();
        TutorialTextForRemainingRubbish.text = $"<color=green>{a.ToString()}</color>"  + " Left";
    }

    public void ActivateRemainingText(bool activate)
    {
        TutorialTextForRemainingRubbish.gameObject.SetActive(activate);

    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class SingleInteractableInfo : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent localizeStringEvent;
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private TextMeshProUGUI _buttonInfoText;

    public void SetButtonInfo(string textKey)
    {
        Debug.Log("debugzort : " + textKey);
        localizeStringEvent.StringReference.TableEntryReference = textKey;
        localizeStringEvent.RefreshString();
    }
   
}

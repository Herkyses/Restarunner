using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInterectableObject
{
    public void InterectableObjectRun();
    public void ShowOutline(bool active);
    public Outline GetOutlineComponent();
    public string GetInterectableText();
    public string[] GetInterectableTexts();
    public string[] GetInterectableButtons();
    public void Open();
    public Enums.PlayerStateType GetStateType();

}

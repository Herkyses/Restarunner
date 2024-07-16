using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInterectableObject
{
    public void InterectableObjectRun();
    public void ShowOutline(bool active);
    public Outline GetOutlineComponent();
    public string GetInterectableText();
    public void Move();
    public void Open();

}

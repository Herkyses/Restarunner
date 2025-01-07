using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cash : MonoBehaviour,IInterectableObject
{
    public float CashValue;
    public Outline CashOutline;


    public void Initiliaze(float cashValue)
    {
        CashValue = cashValue;
    }
    public void InterectableObjectRun()
    {
        GameManager.PayedOrderBill?.Invoke(CashValue);
        GameVfxManager.Instance.SpawnVFX(GameVfxManager.Instance.vfxPools[2].vfxPrefab, transform.position + transform.up*0.25f, transform.rotation);
        PoolManager.Instance.ReturnToPoolForCash(gameObject);
    }
    public void ShowOutline(bool active)
    {
        CashOutline.enabled = active;
    }
    public Outline GetOutlineComponent()
    {
        return CashOutline;

    }
    public string GetInterectableText()
    {
        return null;

    }
    public string[] GetInterectableTexts()
    {
        return null;
    }
    public string[] GetInterectableButtons()
    {
        return null;
    }
   
    public void Open(){
        
    }
    public Enums.PlayerStateType GetStateType()
    {
        return Enums.PlayerStateType.Free;
    }
}

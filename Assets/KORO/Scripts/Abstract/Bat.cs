using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Bat : Tool
{
    [SerializeField] private GameObject _batject;
    private Enums.PlayerStateType firstState;

    public override void Use()
    {
        Player.Instance.StartFight();
    }
    public override void Equip()
    {
        _batject.SetActive(true);
        _batject.transform.SetParent(CameraController.Instance.FightToolChild.transform);
        
        //_batject.transform.localPosition = new Vector3(-0.4f, -0.02f, -0.3f);
        
        
        _batject.transform.localPosition = new Vector3(1f, -0.02f, -0.3f);
        _batject.transform.localRotation = Quaternion.Euler(new Vector3(63,336,6));

        _batject.transform.DOLocalMoveX( -0.4f,0.2f);
        firstState = Player.Instance.PlayerStateType;
        Player.Instance.PlayerStateType = Enums.PlayerStateType.Fight;
    }
    public override void Unequip()
    {
        Player.Instance.PlayerStateType = firstState;
        _batject.transform.DOLocalMoveX( 1f,0.2f).OnComplete(ObjectDeactive);

    }
    
    public void ObjectDeactive()
    {
        _batject.SetActive(false);

    }
}

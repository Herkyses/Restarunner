using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Broom : Tool
{
    [SerializeField] private GameObject _broomObject;
    private Enums.PlayerStateType firstState;
    public override void Use()
    {
        Player.Instance.StartClean();
    }
    public override void Equip()
    {
        _broomObject.SetActive(true);
        _broomObject.transform.SetParent(CameraController.Instance.CleanToolChild.transform);
        _broomObject.transform.localPosition = new Vector3(-0.01f, -0.84f, 0.62f);
        _broomObject.transform.DOLocalMoveZ( -0.05f,0.2f);
        //new Vector3(-0.01f, -0.84f, -0.05f);
        _broomObject.transform.localRotation = Quaternion.Euler(new Vector3(-71,190,260));

        firstState = Player.Instance.PlayerStateType;
        Player.Instance.PlayerStateType = Enums.PlayerStateType.Cleaner;
        //CameraController.Instance.StateInitiliazeForTakeObject(Enums.PlayerStateType.Cleaner);
    }
    public override void Unequip()
    {
        _broomObject.transform.DOLocalMoveZ( 0.62f,0.2f).OnComplete(ObjectDeactive);
        transform.SetParent(Player.Instance.transform);
        Player.Instance.PlayerStateType = firstState;
        //_broomObject.SetActive(false);
    }

    public void ObjectDeactive()
    {
        _broomObject.SetActive(false);

    }
}

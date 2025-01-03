using System.Collections;
using System.Collections.Generic;
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
        _broomObject.transform.localPosition = Vector3.zero;

        firstState = Player.Instance.PlayerStateType;
        Player.Instance.PlayerStateType = Enums.PlayerStateType.Cleaner;
        //CameraController.Instance.StateInitiliazeForTakeObject(Enums.PlayerStateType.Cleaner);
    }
    public override void Unequip()
    {
        transform.SetParent(Player.Instance.transform);
        Player.Instance.PlayerStateType = firstState;
        _broomObject.SetActive(false);
    }
}

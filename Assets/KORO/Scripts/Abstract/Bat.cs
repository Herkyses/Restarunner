using System.Collections;
using System.Collections.Generic;
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
        _batject.transform.localPosition = Vector3.zero;
        firstState = Player.Instance.PlayerStateType;
        Player.Instance.PlayerStateType = Enums.PlayerStateType.Fight;
    }
    public override void Unequip()
    {
        Player.Instance.PlayerStateType = firstState;
        _batject.SetActive(false);
    }
}

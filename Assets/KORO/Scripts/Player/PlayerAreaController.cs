using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAreaController : MonoBehaviour
{
    private Player _player;
    private PlayerMovementController _playerMovementController;
    private PlayerStateController _playerStateController;
    private PlayerPlayingController _playerPlayingController;

    public bool CanShowCanvas;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out _player);
        TryGetComponent(out _playerMovementController);
        TryGetComponent(out _playerPlayingController);
    }
    
    
}

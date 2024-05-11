using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlayingController : MonoBehaviour
{
    
    [SerializeField] private PlayerAnimationController _playerAnimationController;
    [SerializeField] private Player _player;
    [SerializeField] private PlayerStateController _playerStateController;
    public bool _playerPlayingGuitar;
    [SerializeField] private bool _canPlay;
    [SerializeField] private Transform _playTransform;
    [SerializeField] private GameObject _guitar;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.TryGetComponent(out _playerAnimationController);
        gameObject.TryGetComponent(out _player);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && _canPlay)
        {
            if (!_playerPlayingGuitar)
            {
                GameSceneCanvas.Instance.UnShowAreaInfo();
                GameSceneCanvas.Instance._musicController.Isplayable = true;
                _playerPlayingGuitar = true;
                _player._canTakeMoney = true;
                if (_playTransform)
                {
                    gameObject.transform.position = _playTransform.position;
                    gameObject.transform.rotation = _playTransform.rotation;
                    _guitar.SetActive(true);
                }
                CameraController.Instance.CanFollowController(false);
            }
            _playerAnimationController.PlayGuitarAnimation();
        }
        if(Input.GetKeyUp(KeyCode.F))
        {
            GameSceneCanvas.Instance._musicController.Isplayable = false;

            CameraController.Instance.CanFollowController(true);
            _playerPlayingGuitar = false;
            _player._canTakeMoney = false;
            _guitar.SetActive(false);


        }
    }
    public void SetPlayTransform(Transform playTransform)
    {
        _playTransform = playTransform;
    }
    public void SetCanPlayer(bool canPlay)
    {
        _canPlay = canPlay;
    }
}

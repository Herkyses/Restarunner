using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private PlayerAnimationController _playerAnimationController;
    [SerializeField] private Player _player;
    [SerializeField] private bool _playerPlayingGuitar;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.TryGetComponent(out _playerAnimationController);
        gameObject.TryGetComponent(out _player);
    }

    public float speed = 5f; // Karakterin hareket hızı

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // Yatay (A/D) giriş
        float verticalInput = Input.GetAxis("Vertical");     // Dikey (W/S) giriş


        if (horizontalInput == 0 && verticalInput == 0 && !_playerPlayingGuitar)
        {
            _playerAnimationController.PlayIdleAnimation();
        }
        else if ((horizontalInput >= 0 || verticalInput >= 0) && !_playerPlayingGuitar)
        {
            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * speed * Time.deltaTime; // Hareket vektörü hesaplanır
            transform.Translate(movement); // Karakteri hareket ettir

            _playerAnimationController.PlayRunAnimation();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!_playerPlayingGuitar)
            {
                _playerPlayingGuitar = true;
                _player._canTakeMoney = true;
                CameraController.Instance.CanFollowController(false);
            }
            _playerAnimationController.PlayGuitarAnimation();
        }
        if(Input.GetKeyUp(KeyCode.F))
        {
            CameraController.Instance.CanFollowController(true);
            _playerPlayingGuitar = false;
            _player._canTakeMoney = false;

        }
    }
}

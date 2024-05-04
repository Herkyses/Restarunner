using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private PlayerAnimationController _playerAnimationController;
    [SerializeField] private Player _player;
    [SerializeField] private PlayerStateController _playerStateController;
    [SerializeField] private PlayerPlayingController _playerPlayingController;
    [SerializeField] private bool _playerPlayingGuitar;
    [SerializeField] private bool _canPlay;
    [SerializeField] private Transform _playTransform;
    [SerializeField] private GameObject _guitar;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.TryGetComponent(out _playerAnimationController);
        gameObject.TryGetComponent(out _player);
        gameObject.TryGetComponent(out _playerPlayingController);
    }

    
    public float speed = 5f; // Karakterin hareket hızı

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); 
        float verticalInput = Input.GetAxis("Vertical");     


        if (horizontalInput == 0 && verticalInput == 0 && !_playerPlayingController._playerPlayingGuitar)
        {
            _playerAnimationController.PlayIdleAnimation();
        }
        else if ((horizontalInput >= 0 || verticalInput >= 0) && !_playerPlayingController._playerPlayingGuitar)
        {
            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * speed * Time.deltaTime; // Hareket vektörü hesaplanır
            transform.Translate(movement); // Karakteri hareket ettir

            _playerAnimationController.PlayRunAnimation();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private PlayerAnimationController _playerAnimationController;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.TryGetComponent(out _playerAnimationController);
    }

    public float speed = 5f; // Karakterin hareket hızı

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // Yatay (A/D) giriş
        float verticalInput = Input.GetAxis("Vertical");     // Dikey (W/S) giriş

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * speed * Time.deltaTime; // Hareket vektörü hesaplanır

        transform.Translate(movement); // Karakteri hareket ettir
        if (horizontalInput == 0 && verticalInput == 0)
        {
            _playerAnimationController.PlayIdleAnimation();
        }
        else if (horizontalInput >= 0 || verticalInput >= 0)
        {
            _playerAnimationController.PlayRunAnimation();
        }
    }
}

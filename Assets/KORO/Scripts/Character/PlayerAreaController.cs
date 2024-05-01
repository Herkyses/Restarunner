using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAreaController : MonoBehaviour
{
    private Player _player;
    private PlayerMovementController _playerMovementController;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out _player);
        TryGetComponent(out _playerMovementController);
    }
    
    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);

        foreach (Collider col in colliders)
        {
            if (col.gameObject.GetComponent<AreaController>())
            {
                col.gameObject.GetComponent<IAreaInfo>().ShowInfo();
                _playerMovementController.SetPlayTransform(col.gameObject.GetComponent<IAreaInfo>().GetPlayTransform());
                _playerMovementController.SetCanPlayer(true);
                return;
            }
            else
            {
                
            }
        }
        GameSceneCanvas.Instance.UnShowAreaInfo();
        _playerMovementController.SetCanPlayer(false);

    }
}

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
    
    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);

        foreach (Collider col in colliders)
        {
            if (col.gameObject.GetComponent<AreaController>())
            {
                if (!_playerPlayingController._playerPlayingGuitar)
                {
                    col.gameObject.GetComponent<IAreaInfo>().ShowInfo();

                }
                _playerPlayingController.SetPlayTransform(col.gameObject.GetComponent<IAreaInfo>().GetPlayTransform());
                _playerPlayingController.SetCanPlayer(true);
                return;
            }
            else
            {
                
            }
        }

        if (!GameSceneCanvas.Instance.CanShowCanvas)
        {
            GameSceneCanvas.Instance.UnShowAreaInfo();

        }
        _playerPlayingController.SetCanPlayer(false);

    }
}

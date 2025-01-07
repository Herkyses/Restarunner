using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycastController : MonoBehaviour
{
    public GameObject TargetPrice;
    public GameObject TargetObject;

    public Outline InterectabelOutline;

    public bool CanCheckHit;
    
    private GameSceneCanvas _gameSceneCanvas;
    public PlayerObjectMoveController _playerObjectMoveController;

    public IInterectableObject Izort;
    public IMovable IMovableObject;

    public bool IsRaycastActive = true;
    public int layerMask ;
    private Vector3 screenCenter;
    private Ray ray ;
    private RaycastHit hit;
    private Player _player;

    public static Action<IMovable> StartedMove;
    

    private void OnEnable()
    {
        StartedMove += StartMoveObject;
    }

    private void OnDisable()
    {
        StartedMove -= StartMoveObject;

    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SendRaycastCoroutine());
        _gameSceneCanvas = GameSceneCanvas.Instance;
        layerMask = ~LayerMask.GetMask("Ground");
        _player = Player.Instance;

    }

    // Update is called once per frame
    

    public IEnumerator SendRaycastCoroutine()
    {
        
        if (IsRaycastActive)
        {
            while (true)
            {
                yield return new WaitForSeconds(0.05f);
                ProcessRaycast();
            }
        }
        
        
    }
    private void ProcessRaycast()
    {
        screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out hit, 10f, layerMask))
        {
            HandleHit(hit, ray);
        }
        else
        {
            if (!Player.Instance.PlayerTakedObject)
            {
                HandleNoHit(ray);
            }
            
        }
    }
    private void HandleHit(RaycastHit hit, Ray ray)
    {
        var hitObject = hit.collider.gameObject;
        if (TargetObject == hitObject)
        {
            if (hitObject.TryGetComponent(out IInterectableObject interactable))
            {
                if (interactable.GetStateType() == Player.Instance.PlayerStateType)
                {
                    return;
                }
                else
                {
                    Debug.Log("targetobjectnull");
                    TargetObject = null;
                }
                //UpdateInterectable(hitObject, interactable);

            }
        }
        if (hitObject.TryGetComponent(out IMovable movable))
        {
            IMovableObject = movable;
            _playerObjectMoveController.InitiliazeMoveableObject(movable.GetMoveableObjectTransform(),movable);
        }

        if (hitObject.TryGetComponent(out IInterectableObject interact) &&
            interact.GetStateType() == Player.Instance.PlayerStateType)
        {
            TargetObject = hitObject;

            UpdateInterectable(interact);
            Debug.Log("Raycast hit: " + hitObject.name);
            Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.green, 2f);
        }
        else
        {
            HandleNonMatchingState();
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2f);
            Debug.Log("objectname:" + hit.collider.gameObject);
        }
    }
    private void HandleNoHit(Ray ray)
    {
        if (InterectabelOutline)
        {
            InterectabelOutline.enabled = false;
            _gameSceneCanvas.CanShowCanvas = false;

            if (Izort != null)
                Izort.ShowOutline(false);

            _gameSceneCanvas.UnShowAreaInfo();
            Debug.Log("unshowedwithraycast");
        }
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2f);
    }
    private void UpdateInterectable(IInterectableObject interact)
    {
        Izort = interact;
        if (InterectabelOutline != null)
        {
            InterectabelOutline.enabled = false;
            _gameSceneCanvas.CanShowCanvas = false;
            _gameSceneCanvas.UnShowAreaInfo();
        }
        InterectabelOutline = Izort.GetOutlineComponent();
        Izort.ShowOutline(true);
        _gameSceneCanvas.CanShowCanvas = true;
        //_gameSceneCanvas.ShowAreaInfo(Izort.GetInterectableText());
        _gameSceneCanvas.ShowAreaInfoForTexts(Izort.GetInterectableTexts());
        _gameSceneCanvas.ShowAreaInfoForTextsButtons(Izort.GetInterectableButtons());
    }

    private void HandleNonMatchingState()
    {
        if (Player.Instance.PlayerStateType != Enums.PlayerStateType.TakeBox)
        {
            _gameSceneCanvas.UnShowAreaInfo();
        }
        Izort = null;
        TargetObject = null;
        DeactivateOutline();
    }

    public void DeactivateOutline()
    {
        if (InterectabelOutline)
        {
            InterectabelOutline.enabled = false;
        }
    }
    private void Update()
    {
        HandleInteractionInput();
        HandleObjectMovement();
        HandleObjectDrop();
    }

    private void HandleInteractionInput()
    {
        if (Cursor.visible)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (_player.GetCurrentTool() != null)
            {
                _player.GetCurrentTool().Use();
            }
            RunInteractableObject();
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            OpenInteractableObject();
        }
    }

    private void RunInteractableObject()
    {
        if (Izort != null)
        {
            Izort.InterectableObjectRun();
        }
    }

    private void OpenInteractableObject()
    {
        if (Izort != null)
        {
            Izort.Open();
        }
    }

    private void HandleObjectMovement()
    {
        
        if (Input.GetKey(KeyCode.H) && Izort != null)
        {
            if (ControllerManager.Instance.PlaceController.IsRestaurantOpen)
            {
                return;
            }

            if (IMovableObject != null)
            {
                StartMoveObject(IMovableObject);
            } 
            //Izort.Move();
        }
    }
    public void StartMoveObject(IMovable iMovable)
    {
        _playerObjectMoveController.InitiliazeMoveableObject(iMovable.GetMoveableObjectTransform(),iMovable);
        iMovable.Movement();
        _playerObjectMoveController.IsCheckAround = true;
    }

    private void HandleObjectDrop()
    {
        if (Input.GetKey(KeyCode.J) && IsPlayerInStateForObjectDrop())
        {
            DropPlayerObject();
        }
    }

    private bool IsPlayerInStateForObjectDrop()
    {
        return Player.Instance.PlayerStateType == Enums.PlayerStateType.TakeBox ||
               Player.Instance.PlayerStateType == Enums.PlayerStateType.TakeFoodIngredient;
    }

    private void DropPlayerObject()
    {
        var takenObject = Player.Instance.PlayerTakedObject;

        if (takenObject)
        {
            DetachAndDropObject(takenObject);
            Player.Instance.TakedObjectNull();
            _gameSceneCanvas.UnShowAreaInfo();
        }
    }

    private void DetachAndDropObject(GameObject takenObject)
    {
        takenObject.transform.SetParent(null);
        takenObject.transform.position = CameraController.Instance.PlayerTakedObjectTransformParent.position;

        if (takenObject.TryGetComponent(out Rigidbody objectRigid))
        {
            objectRigid.useGravity = true;
            objectRigid.freezeRotation = false;
        }

        if (takenObject.TryGetComponent(out BoxCollider objectCollider))
        {
            objectCollider.enabled = true;
        }
    }
}

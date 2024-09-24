using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycastController : MonoBehaviour
{
    public GameObject TargetPrice;

    public Outline InterectabelOutline;

    private GameSceneCanvas _gameSceneCanvas;

    public IInterectableObject Izort;

    public bool IsRaycastActive = true;
    public int layerMask = ~LayerMask.GetMask("Ground");
    private Vector3 screenCenter;
    private Ray ray ;
    private RaycastHit hit;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SendRaycastCoroutine());
        _gameSceneCanvas = GameSceneCanvas.Instance;
        layerMask = ~LayerMask.GetMask("Ground");


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
            HandleNoHit(ray);
        }
    }
    private void HandleHit(RaycastHit hit, Ray ray)
    {
        var hitObject = hit.collider.gameObject;

        if (hitObject.TryGetComponent(out IInterectableObject interact) &&
            interact.GetStateType() == Player.Instance.PlayerStateType)
        {
            UpdateInterectable(hitObject, interact);
            Debug.Log("Raycast hit: " + hitObject.name);
            Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.green, 2f);
        }
        else
        {
            HandleNonMatchingState();
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
        }
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2f);
    }
    private void UpdateInterectable(GameObject hitObject, IInterectableObject interact)
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
        _gameSceneCanvas.ShowAreaInfo(Izort.GetInterectableText());
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
        if (Input.GetKeyUp(KeyCode.E))
        {
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
            Izort.Move();
        }
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
        takenObject.transform.position = transform.position + Vector3.up;

        if (takenObject.TryGetComponent(out Rigidbody objectRigid))
        {
            objectRigid.useGravity = true;
        }

        if (takenObject.TryGetComponent(out BoxCollider objectCollider))
        {
            objectCollider.enabled = true;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationObject : MonoBehaviour,IInterectableObject
{
    private bool isDecorationMove;
    private bool isDecorationCanSet;

    private int layerMask;
    private int layerToIgnore;
    public int groundLayer;
    public int decorationLayer;
    public int decorationID;
    
    [SerializeField] private string[] textsForInteractable = new [] {"Move"};
    [SerializeField] private string[] textsForMove = new [] {"Drop","Rotate"};
    [SerializeField] private string[] textsButtons = new [] {"H"};
    [SerializeField] private string[] textsButtonsForMove = new [] {"M0","R"};

    private GameSceneCanvas _gameSceneCanvas;
    
    
    [SerializeField] private Outline _outline;




    private void Start()
    {
        groundLayer = LayerMask.NameToLayer("Ground");
        decorationLayer = LayerMask.NameToLayer("Decoration");
        textsForMove = new [] {"Set up","Rotate"};
        textsForInteractable = new [] {"Move"};
        textsButtons = new [] {"H"};
        textsButtonsForMove = new [] {"M0","R"};
        _outline = GetComponent<Outline>();
        _gameSceneCanvas = GameSceneCanvas.Instance;
    }

    private void Update()
    {
        if (isDecorationMove)
        {
            MoveStart();
        }
    }

    public void InterectableObjectRun()
    {
        
    }
    public void ShowOutline(bool active)
    {
        _outline.enabled = active;
    }
    public Outline GetOutlineComponent()
    {
        return _outline;
    }
    public string GetInterectableText()
    {
        return null;

    }
    public string[] GetInterectableTexts()
    {
        return textsForInteractable;

    }
    public string[] GetInterectableButtons()
    {
        return textsButtons;

    }
    public void Move()
    {
        if (!isDecorationMove && !ControllerManager.Instance.PlaceController.RestaurantIsOpen)
        {
            
            //gameObject.layer = LayerMask.NameToLayer("Ground");
            GetComponent<BoxCollider>().enabled = false;
            _gameSceneCanvas.MoveObjectInfo(textsForMove,textsButtonsForMove,Enums.PlayerStateType.DecorationMove);
            isDecorationMove = true;
            
        }
    }
    public void MoveStart()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        //Player.Instance.DeactivatedRaycast();
        
        if (Physics.Raycast(ray, out hit))
        {
            float xValue = hit.point.x;
            float zValue = hit.point.z;
            //GetComponent<BoxCollider>().enabled = true;
            transform.position = new Vector3(xValue,0,zValue); // Objenin pozisyonunu fare ile tıklanan noktaya taşı
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            var tableRotat = transform.rotation;
            var tableRotatTemp = Quaternion.Euler(new Vector3(tableRotat.eulerAngles.x,tableRotat.eulerAngles.y+90f,tableRotat.eulerAngles.z));

            transform.rotation = tableRotatTemp;
        }
        if (Input.GetMouseButton(0))
        {
            
            isDecorationMove = false;
            Player.Instance.PlayerStateType = Enums.PlayerStateType.Free;
            _gameSceneCanvas.CheckShowInfoText = true;
            MapManager.Instance.SaveMap();
            gameObject.transform.SetParent(DecorationController.Instance.transform);
            ControllerManager.Instance.PlaceController.ActivateDecorationPlane(false);
            GetComponent<BoxCollider>().enabled = true;


        }
        
    }
    public void CheckGround()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.8f);
        var checkControl = false;
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.gameObject.layer == decorationLayer)
            {
                return;
            }
           
        }
        foreach (Collider collider in colliders)
        {
            
            if (collider.gameObject != gameObject && collider.gameObject.layer == groundLayer) 
            {
                isDecorationCanSet = true;
                return;
            }
        }

       

        isDecorationCanSet = false;
    }
    public void Open()
    {
        
    }
    public Enums.PlayerStateType GetStateType()
    {
        return Enums.PlayerStateType.Free;
    }
}

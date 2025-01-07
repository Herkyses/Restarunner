using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationObject : MonoBehaviour,IInterectableObject,IMovable
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

 

    public void Open()
    {
        
    }
    public Enums.PlayerStateType GetStateType()
    {
        return Enums.PlayerStateType.Free;
    }

    public void Movement()
    {
        _gameSceneCanvas.MoveObjectInfo(textsForMove,textsButtonsForMove,Enums.PlayerStateType.DecorationMove,gameObject);

    }
    public void PlacedObject()
    {
        isDecorationMove = false;
        Player.Instance.TakedObjectNull();
        _gameSceneCanvas.CheckShowInfoText = true;
        MapManager.Instance.SaveMap();
        gameObject.transform.SetParent(DecorationController.Instance.transform);
        ControllerManager.Instance.PlaceController.ActivateDecorationPlane(false);
        GetComponent<BoxCollider>().enabled = true;
    }
    public Transform GetMoveableObjectTransform()
    {
        return null;
    }
    public Renderer[] GetRenderers()
    {
        return null;

    }
}

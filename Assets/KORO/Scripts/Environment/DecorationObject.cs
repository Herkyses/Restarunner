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



    private void Start()
    {
        groundLayer = LayerMask.NameToLayer("Ground");
        decorationLayer = LayerMask.NameToLayer("Decoration");
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
        
    }
    public Outline GetOutlineComponent()
    {
        return null;
    }
    public string GetInterectableText()
    {
        return null;

    }
    public string[] GetInterectableTexts()
    {
        return null;

    }
    public string[] GetInterectableButtons()
    {
        return null;

    }
    public void Move()
    {
        if (!isDecorationMove && !PlaceController.RestaurantIsOpen)
        {
            
            //gameObject.layer = LayerMask.NameToLayer("Ground");
            GetComponent<BoxCollider>().enabled = false;
            Player.Instance.PlayerTakedObject = gameObject;
            Player.Instance.PlayerStateType = Enums.PlayerStateType.DecorationMove;
            Player.Instance.ActivatedRaycast(false);
            isDecorationMove = true;


            //GameSceneCanvas.Instance.ShowAreaInfoForTexts(textsForTable);
            //GameSceneCanvas.Instance.ShowAreaInfoForTextsButtons(textsButtonsForTable);
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
            //TableSet.CheckGround();
            /*if (IsTableSetTransform)
            {
                MapManager.Instance.SaveMap();
                TableControl();
                Player.Instance.ActivatedRaycast(true);

                Player.Instance.PlayerStateType = Enums.PlayerStateType.Free;
                //if(colliders.Length )
                TableSet.GetComponent<BoxCollider>().enabled = false;
                TableController.Instance.EnableTableSetCollider(false);
                IsTableSetTransform = false;
                IsTableMove = false;
                Player.Instance.PlayerTakedObject = null;
                if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 2)
                {
                    PlayerPrefsManager.Instance.SavePlayerPlayerTutorialStep(4);
                    TutorialManager.Instance.Initiliaze();
                }
            }*/
            isDecorationMove = false;
            Player.Instance.PlayerStateType = Enums.PlayerStateType.Free;
            MapManager.Instance.SaveMap();
            gameObject.transform.SetParent(DecorationController.Instance.transform);

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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycastController : MonoBehaviour
{
    public GameObject TargetPrice;

    public Outline InterectabelOutline;

    public IInterectableObject Izort;

    public bool IsRaycastActive = true;
    
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SendRaycastCoroutine());
    }

    // Update is called once per frame
    

    public IEnumerator SendRaycastCoroutine()
    {
        int layerToIgnore = LayerMask.NameToLayer("Ground"); 
        int layerMask = 1 << layerToIgnore;
        layerMask = ~layerMask;
        if(IsRaycastActive)
        {
            while (true)
        {
            
            yield return new WaitForSeconds(0.05f);
            //if (!Player.Instance.PlayerTakedObject)
            //{
                Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);


                Ray ray = Camera.main.ScreenPointToRay(screenCenter);
            
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit,10f,layerMask))
                {
                    var hitObject = hit.collider.gameObject;
                    if(/*(hitObject.TryGetComponent(out Rubbish rubbish) && Player.Instance.CanCleanRubbish)
                       ||(!Player.Instance.CanCleanRubbish&& !PlayerOrderController.Instance.TakedFood)
                       ||(hitObject.TryGetComponent(out AIController aicon) && PlayerOrderController.Instance.TakedFood)*/ hitObject.TryGetComponent(out IInterectableObject interact))
                    {
                        if (interact.GetStateType() == Player.Instance.PlayerStateType)
                        {
                            
                            var gameScene = GameSceneCanvas.Instance;

                            Izort = hit.collider.gameObject.GetComponent<IInterectableObject>();
                            if (Izort != null)
                            {
                    
                                if (InterectabelOutline)
                                {
                                    InterectabelOutline.enabled = false;
                                    gameScene.CanShowCanvas = false;

                                    gameScene.UnShowAreaInfo();
                                }

                                InterectabelOutline = Izort.GetOutlineComponent();
                                Izort.ShowOutline(true);
                                gameScene.CanShowCanvas = true;
                                gameScene.ShowAreaInfo(Izort.GetInterectableText());
                                gameScene.ShowAreaInfoForTexts(Izort.GetInterectableTexts());
                                gameScene.ShowAreaInfoForTextsButtons(Izort.GetInterectableButtons());
                                //place.ShowPlacePrice();
                            }
                            else
                            {
                                gameScene.CanShowCanvas = false;
                                if (Izort != null)
                                {
                                    Izort.ShowOutline(false);
                                }

                                DeactivateOutline();

                                gameScene.UnShowAreaInfo();
                    
                            }
                            // Raycast sonucunu işle
                            Debug.Log("Raycast isabet etti: " + hit.collider.gameObject.name);
                            Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.green, 2f);
                        
                        }
                        else
                        {
                            if (Player.Instance.PlayerStateType != Enums.PlayerStateType.TakeBox)
                            {
                                GameSceneCanvas.Instance.UnShowAreaInfo();

                            }
                            Izort = null;
                            DeactivateOutline();
                        }
                        
                    }
                    else
                    {
                        if (Player.Instance.PlayerStateType != Enums.PlayerStateType.TakeBox && Player.Instance.PlayerStateType != Enums.PlayerStateType.MoveTable)
                        {
                            GameSceneCanvas.Instance.UnShowAreaInfo();
                        }
                        Izort = null;
                        DeactivateOutline();
                        InterectabelOutline = null;
                    }
                    

                }
                else
                {
                    if (InterectabelOutline)
                    {
                        InterectabelOutline.enabled = false;
                        GameSceneCanvas.Instance.CanShowCanvas = false;
                        if(Izort != null)
                            Izort.ShowOutline(false);

                        GameSceneCanvas.Instance.UnShowAreaInfo();
                    }
                    // Raycast hiçbir şeye isabet etmedi
                    Debug.Log("Raycast hiçbir şeye isabet etmedi.");
                    Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2f);

                }
            //}
           

            
        }
            
        }
        
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
        
        if(Input.GetKeyUp(KeyCode.E))
        {
            if (Izort != null)
            {
                Izort.InterectableObjectRun();

            }
        }
        if(Input.GetKeyUp(KeyCode.T))
        {
            if (Izort != null)
            {
                Izort.Open();

            }
        }

        if (Input.GetKey(KeyCode.H))
        {
            if (Izort != null)
            {
                Izort.Move();
            }
        }
        if (Input.GetKey(KeyCode.J))
        {
            if (Player.Instance.PlayerStateType == Enums.PlayerStateType.TakeBox)
            {
                var takenObject = Player.Instance.PlayerTakedObject;
                if (takenObject)
                {
                    takenObject.transform.SetParent(null);
                    if (takenObject.GetComponent<Rigidbody>())
                    {
                        takenObject.GetComponent<Rigidbody>().useGravity = true;
                    }
                    if (takenObject.GetComponent<BoxCollider>())
                    {
                        takenObject.GetComponent<BoxCollider>().enabled = true;
                    }

                    var playerInstance = Player.Instance;
                    playerInstance.PlayerTakedObject = null;
                    GameSceneCanvas.Instance.UnShowAreaInfo();
                    playerInstance.PlayerStateType = Enums.PlayerStateType.Free;
                }
            }
            
        }
    }
}

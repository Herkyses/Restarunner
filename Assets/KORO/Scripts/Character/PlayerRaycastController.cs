using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycastController : MonoBehaviour
{
    public GameObject TargetPrice;

    public Outline InterectabelOutline;

    public IInterectableObject Izort;
    
    
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
        
        while (true)
        {
            
            yield return new WaitForSeconds(0.05f);
            if (!Player.Instance.PlayerTakedObject)
            {
                Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);


            Ray ray = Camera.main.ScreenPointToRay(screenCenter);
            
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,10f,layerMask))
            {
                
                Izort = hit.collider.gameObject.GetComponent<IInterectableObject>();
                if (Izort != null)
                {
                    
                    if (InterectabelOutline)
                    {
                        InterectabelOutline.enabled = false;
                        GameSceneCanvas.Instance.CanShowCanvas = false;

                        GameSceneCanvas.Instance.UnShowAreaInfo();
                    }

                    InterectabelOutline = Izort.GetOutlineComponent();
                    Izort.ShowOutline(true);
                    GameSceneCanvas.Instance.CanShowCanvas = true;
                    GameSceneCanvas.Instance.ShowAreaInfo(Izort.GetInterectableText());
                    GameSceneCanvas.Instance.ShowAreaInfoForTexts(Izort.GetInterectableTexts());
                    GameSceneCanvas.Instance.ShowAreaInfoForTextsButtons(Izort.GetInterectableButtons());
                    //place.ShowPlacePrice();
                }
                else
                {

                    GameSceneCanvas.Instance.CanShowCanvas = false;
                    if (Izort != null)
                    {
                        Izort.ShowOutline(false);
                    }

                    if (InterectabelOutline)
                    {
                        InterectabelOutline.enabled = false;

                    }

                    GameSceneCanvas.Instance.UnShowAreaInfo();
                    
                }
                // Raycast sonucunu işle
                Debug.Log("Raycast isabet etti: " + hit.collider.gameObject.name);
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.green, 2f);

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
            }
           

            
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) )
        {
        }
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
            if (Player.Instance.PlayerTakedObject)
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
                    
                    Player.Instance.PlayerTakedObject = null;
                    GameSceneCanvas.Instance.UnShowAreaInfo();
                }
            }
            
        }
    }
}

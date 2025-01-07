using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class OrderBox : MonoBehaviour,IInterectableObject
{
    
    [SerializeField] private string[] texts = new [] {"Take OrderBox"};
    [SerializeField] private string[] textsBefore = new [] {"Take OrderBox"};
    [SerializeField] private string[] textsForTake = new [] {"Drop"};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    [SerializeField] private string[] textsButtonsBefore = new [] {"E"};
    [SerializeField] private string[] textsButtonsForTake = new [] {"J"};
    [SerializeField] private Image ordeBoxItemIcon;
    [SerializeField] private ShopItemData _shopItemData;
    [SerializeField] private bool _isOrderBoxOpenAvailable;
    
    private Outline _outline;
    private Rigidbody _rigidbody;
    public string[] InteractableButtons;
    public int groundLayer;
    public int toolLayer;
    // Start is called before the first frame update
    void Start()
    {
        groundLayer = LayerMask.NameToLayer("Ground");
        toolLayer = LayerMask.NameToLayer("Tool");
        texts = new [] {"Key_Interactable_OB_Take","Key_Open"};
        textsBefore = new [] {"Key_Interactable_OB_Take"};
        
        textsButtons = new [] {"E","T"};
        textsButtonsForTake = new [] {"J","O"};
        textsButtonsBefore = new [] {"E"};
        textsForTake = new [] {"Key_Drop","Key_Open"};
        _outline = GetComponent<Outline>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public ShopItemData GetShopItemData()
    {
        return _shopItemData;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetShopItemData(ShopItemData shopItemData)
    {
        _shopItemData = shopItemData;
        ordeBoxItemIcon.sprite = shopItemData.ShopItemIcon;
    }

    public void InterectableObjectRun()
    {
        _rigidbody.useGravity = false;
        _rigidbody.freezeRotation = true;
        _rigidbody.velocity = Vector3.zero;
        GetComponent<BoxCollider>().enabled = false;
        transform.DOLocalMove(new Vector3(-0.1f,-0.2f,0f), 0.2f);
        transform.DOLocalRotate(new Vector3(0,0,-11f), 0.2f);
        transform.SetParent(CameraController.Instance.PlayerTakedObjectTransformParent);
        
        Player.Instance.TakedObject(gameObject,Enums.PlayerStateType.TakeBox);
        GameSceneCanvas.Instance.ShowAreaInfoForTexts(textsForTake);
        GameSceneCanvas.Instance.ShowAreaInfoForTextsButtons(textsButtonsForTake);
        _isOrderBoxOpenAvailable = false;
        //gameObject.layer = toolLayer;
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
        if (_isOrderBoxOpenAvailable)
        {
            return texts;
        }
        return textsBefore;
    }
    
    
    //TODO: hardcode
    public void Open()
    {
        
        if (!Player.Instance.PlayerTakedObject)
        {
            var instantiatedObject = Instantiate(_shopItemData.ItemObject);
            instantiatedObject.transform.position = transform.position;
            HandleShopItem(_shopItemData, instantiatedObject);

            if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 3)
            {
                TutorialManager.Instance.SetTutorialInfo(4);
            }

            ResetOrderBox();
            PoolManager.Instance.ReturnToPoolForOrderBox(gameObject);
        }
        
    }
    public void HandleShopItem(ShopItemData shopItemData, GameObject instantiatedObject)
    {
        switch (shopItemData.ItemType)
        {
            case Enums.ShopItemType.Table:
                HandleTable(instantiatedObject);
                break;
            case Enums.ShopItemType.Decoration:
                HandleDecoration(instantiatedObject);
                break;
            case Enums.ShopItemType.FoodIngredient:
                HandleFoodIngredient(shopItemData, instantiatedObject);
                break;
        }
    }

    //TODO: TABLE A SİNGLERESPONSABİLİTY
    private void HandleTable(GameObject tableObject)
    {
        var controllerManager = ControllerManager.Instance;
        if (controllerManager.PlaceController.IsRestaurantOpen)
            return;
        var tableObjectForInject = tableObject.GetComponent<TableSet>().table;
        controllerManager.Tablecontroller.InjectTableObject(tableObjectForInject);
        PlayerRaycastController.StartedMove?.Invoke(tableObjectForInject.GetComponent<IMovable>());
    }

    private void HandleDecoration(GameObject decorationObject)
    {
        decorationObject.GetComponent<DecorationObject>()?.Move();
    }

    private void HandleFoodIngredient(ShopItemData shopItemData, GameObject crateObject)
    {
        var crate = crateObject.GetComponent<SingleCrate>();
        if (crate != null)
        {
            crate.Initiliaze(shopItemData);
            crate.InterectableObjectRun();
        }
    }

    public void ResetOrderBox()
    {
        _rigidbody.useGravity = true;
        GetComponent<BoxCollider>().enabled = true;
        _isOrderBoxOpenAvailable = true;

    }
    public string[] GetInterectableButtons()
    {
        if (_isOrderBoxOpenAvailable)
        {
            return textsButtons;
        }
        return textsButtonsBefore;
    }

    public Enums.PlayerStateType GetStateType()
    {
        return Enums.PlayerStateType.Free;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == groundLayer)
        {
            _isOrderBoxOpenAvailable = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.layer == groundLayer)
        {
            _isOrderBoxOpenAvailable = false;
        }    
    }
    private void OnDrawGizmos()
    {
        // Gizmos ile tetiklenen objeyi ve tetikleme durumunu görebilirsin
        Gizmos.color = _isOrderBoxOpenAvailable ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
    
}

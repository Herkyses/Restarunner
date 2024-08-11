using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class OrderBox : MonoBehaviour,IInterectableObject
{
    
    [SerializeField] private string[] texts = new [] {"Take OrderBox"};
    [SerializeField] private string[] textsBefore = new [] {"Take OrderBox"};
    [SerializeField] private string[] textsForTake = new [] {"Drop"};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    [SerializeField] private string[] textsButtonsBefore = new [] {"E"};
    [SerializeField] private string[] textsButtonsForTake = new [] {"J"};
    [SerializeField] private ShopItemData _shopItemData;
    [SerializeField] private bool _isOrderBoxOpenAvailable;
    private Outline _outline;
    public string[] InteractableButtons;
    public int groundLayer;
    // Start is called before the first frame update
    void Start()
    {
        groundLayer = LayerMask.NameToLayer("Ground");
        texts = new [] {"Take OrderBox","Open"};
        textsBefore = new [] {"Take OrderBox"};
        
        textsButtons = new [] {"E","T"};
        textsButtonsForTake = new [] {"J"};
        textsButtonsBefore = new [] {"E"};
        textsForTake = new [] {"Drop"};
        _outline = GetComponent<Outline>();
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
    }

    public void InterectableObjectRun()
    {
        
        transform.DOLocalMove(new Vector3(-0.1f,-0.2f,0f), 0.2f);
        transform.DOLocalRotate(new Vector3(0,0,-11f), 0.2f);
        transform.SetParent(CameraController.Instance.PlayerTakedObjectTransformParent);
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<BoxCollider>().enabled = false;
        Player.Instance.PlayerTakedObject = gameObject;
        Player.Instance.PlayerStateType = Enums.PlayerStateType.TakeBox;
        GameSceneCanvas.Instance.ShowAreaInfoForTexts(textsForTake);
        GameSceneCanvas.Instance.ShowAreaInfoForTextsButtons(textsButtonsForTake);
        _isOrderBoxOpenAvailable = false;
        
        
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

    public void Move()
    {
        
    }
    public void Open()
    {
        if (!Player.Instance.PlayerTakedObject && _isOrderBoxOpenAvailable)
        {
            var objectZort = Instantiate(_shopItemData.ItemObject);
            objectZort.transform.position = new Vector3(objectZort.transform.position.x, 0, objectZort.transform.position.z);
            PoolManager.Instance.ReturnToPoolForOrderBox(gameObject);
            if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 2)
            {
                TutorialManager.Instance.SetTutorialInfo(3);
            }

            if (_shopItemData.ItemType == Enums.ShopItemType.Table)
            {
                objectZort.GetComponent<TableSet>().table.Move();
            }
            if (_shopItemData.ItemType == Enums.ShopItemType.Decoration)
            {
                objectZort.GetComponent<DecorationObject>().Move();
            }
        }
        
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

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class OrderBox : MonoBehaviour,IInterectableObject
{
    
    [SerializeField] private string[] texts = new [] {"Take OrderBox"};
    [SerializeField] private string[] textsForTake = new [] {"Drop"};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    [SerializeField] private string[] textsButtonsForTake = new [] {"J"};
    [SerializeField] private ShopItemData _shopItemData;

    public string[] InteractableButtons;
    // Start is called before the first frame update
    void Start()
    {
        texts = new [] {"Take OrderBox","Open"};
        textsButtons = new [] {"E","T"};
        textsButtonsForTake = new [] {"J"};
        textsForTake = new [] {"Drop"};
        
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
        if (!Player.Instance.PlayerTakedObject)
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
        }
        
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
        return texts;
    }

    public void Move()
    {
        
    }
    public void Open()
    {
        if (!Player.Instance.PlayerTakedObject)
        {
            var objectZort = Instantiate(_shopItemData.ItemObject);
            objectZort.transform.position = transform.position;
            PoolManager.Instance.ReturnToPoolForOrderBox(gameObject);
            if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 2)
            {
                TutorialManager.Instance.SetTutorialInfo(3);
            }
        }
        
    }
    public string[] GetInterectableButtons()
    {
        return textsButtons;
    }

    public Enums.PlayerStateType GetStateType()
    {
        return Enums.PlayerStateType.Free;
    }
}

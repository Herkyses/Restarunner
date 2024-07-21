using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class OrderBox : MonoBehaviour,IInterectableObject
{
    [SerializeField] private ShopItemData _shopItemData;

    public string[] InteractableButtons;
    // Start is called before the first frame update
    void Start()
    {
        
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
        return null;
    }

    public void Move()
    {
        
    }
    public void Open()
    {
        var objectZort = Instantiate(_shopItemData.ItemObject);
        objectZort.transform.position = transform.position;
        PoolManager.Instance.ReturnToPoolForOrderBox(gameObject);
    }
    public string[] GetInterectableButtons()
    {
        return null;
    }
}

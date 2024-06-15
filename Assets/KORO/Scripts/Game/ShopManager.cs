using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public List<ShopItem> FirstShopItems;
    public List<ShopItemData> FirstShopItemDatas;
    public List<ShopItemData> EnvironmentShopItemDatas;
    public Transform ShopOrderTransform;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void CreateShopItem(ShopItemData shopItemData)
    {
        switch (shopItemData.ItemType)
        {
            case Enums.ShopItemType.Chef:
                break;
            case Enums.ShopItemType.Table:
                CreateTable(shopItemData);
                break;
            case Enums.ShopItemType.Waiter:
                break;
            
        }
    }

    public void CreateTable(ShopItemData shopItemData)
    {
        var item = Instantiate(shopItemData.ItemObject);
        item.transform.position = ShopOrderTransform.position;
        
    }

    
}
[System.Serializable]
public class ShopItem
{
    public Enums.ShopItemType ItemType;
    public float ShopItemPrice;
    public Sprite ShopItemIcon;
    public GameObject ItemObject;
}

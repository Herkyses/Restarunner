using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "Shop/ShopItemData")]

public class ShopItemData : ScriptableObject
{
    public Enums.ShopItemType ItemType;
    public Enums.ShopItemUIType ShopItemUIType;
    public float ShopItemPrice;
    public Sprite ShopItemIcon;
    public GameObject ItemObject;
    public Enums.OrderType ItemOrderType;
    public int TableSetID;
    public int PlaceLevel;
    public int DecorationID;

}

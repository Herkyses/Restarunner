using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public enum AreaStateType
    {
        Door = 0,
        PlayArea = 1,
        
    }
    public enum OrderType
    {
        Pizza,
        Burger,
        Taco,
        Croissant,
        HotDog,
        Sandwich,
        Chicken,
    }
    public enum SingleOrderUIType
    {
        Order,
        FoodList,
        PlayerOrderList,
        
    }
    public enum ShopItemType
    {
        Chef,
        Table,
        Waiter,
        FoodIngredient,
        PlaceUpgrade,
        Decoration,
    }

    public enum ShopItemUIType
    {
        Shop,
        Inventory,
    }

    public enum AIStateType
    {
        Waiter,
        Customer,
    }
    public enum PlayerStateType
    {
        Free,
        OrderBill,
        Cleaner,
        TakeBox,
        Trays,
        GiveFood,
        MoveTable,
        Fight,
    }
}

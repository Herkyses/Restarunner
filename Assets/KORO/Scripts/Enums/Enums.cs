using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    
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
    public enum FoodIngredientType
    {
        Cheese,
        Meat,
        Lettuce,
        Tomato,
        Onion,
        ChickenBreast,
        Bread,
        Flour,
        Eggs,
        Milk,
        OtherVegetables,
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
        None,
        DecorationMove,
        TakeFoodIngredient,
    }
    public enum BillButtonType
    {
        Clear,
        Delete,
        Number,
        Zero,
    }
    public enum LanguageType
    {
        English = 0,
        Spanish = 1,
        Turkish = 2,
        Chinesee = 3,
    }
    public enum ToolType
    {
        Sweeper, 
        Bat      
    }
}

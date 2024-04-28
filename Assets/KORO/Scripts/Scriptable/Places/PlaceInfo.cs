using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewPlaceInfo", menuName = "Places/PlaceInfo")]
public class PlaceInfo : ScriptableObject
{
    public string placeName;
    public string description;
    public Sprite icon;
    public int capacity;
    public float pricePerPerson;
    public float Price;
    public bool isOpen;

    public void Initialize(string name, string desc, Sprite icon, int capacity, float price, bool open)
    {
        placeName = name;
        description = desc;
        this.icon = icon;
        this.capacity = capacity;
        pricePerPerson = price;
        isOpen = open;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBox : MonoBehaviour
{
    [SerializeField] private ShopItemData _shopItemData;
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
}

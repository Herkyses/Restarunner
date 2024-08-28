using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleCrate : MonoBehaviour
{
    [SerializeField] private Transform _ingredientTransform;
    [SerializeField] private int _ingredientColumnCount;
    [SerializeField] private int _ingredientRowCount;
    [SerializeField] private List<GameObject> _ingredientList;
    [SerializeField] private List<Transform> _ingredientTransformsList;
    [SerializeField] private ShopItemData _shopItemData;
    [SerializeField] private Enums.FoodIngredientType _shopItemDataIngredientType;
    // Start is called before the first frame update


    public void Initiliaze(ShopItemData shopItemData)
    {
        _shopItemData = shopItemData;
        _shopItemDataIngredientType = shopItemData.FoodIngredientType;
        for (int i = 0; i < _shopItemData.ItemCount; i++)
        {
            var singleIngredient = PoolManager.Instance.GetFromPoolForFoodIngredient(); //getfrom pool 
            singleIngredient.transform.SetParent(transform);
            singleIngredient.transform.position = _ingredientTransformsList[i].position;
            singleIngredient.transform.localScale = Vector3.one;
            singleIngredient.GetComponent<MeshFilter>().sharedMesh = GameDataManager.Instance.GetFoodIngredientMeshFilter(_shopItemDataIngredientType).sharedMesh;
            //singleIngredient.Get
        }
    }
}

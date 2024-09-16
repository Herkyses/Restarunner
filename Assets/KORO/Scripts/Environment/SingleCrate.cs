using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SingleCrate : MonoBehaviour,IInterectableObject
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
            _ingredientList.Add(singleIngredient);
            singleIngredient.transform.SetParent(transform);
            singleIngredient.transform.position = _ingredientTransformsList[i].position;
            singleIngredient.transform.localScale = Vector3.one;
            singleIngredient.GetComponent<MeshFilter>().sharedMesh = GameDataManager.Instance.GetFoodIngredientMeshFilter(_shopItemDataIngredientType).sharedMesh;
            singleIngredient.GetComponent<SingleIngredient>().IngredientType = _shopItemDataIngredientType;
            //singleIngredient.Get
        }
    }

    public void InterectableObjectRun()
    {
        transform.DOLocalMove(new Vector3(-0.1f,-0.2f,0f), 0.2f);
        transform.DOLocalRotate(new Vector3(0,0,-11f), 0.2f);
        transform.SetParent(CameraController.Instance.PlayerTakedObjectTransformParent);
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<BoxCollider>().enabled = false;
        Player.Instance.TakedObject(gameObject,Enums.PlayerStateType.TakeFoodIngredient);
        //GameSceneCanvas.Instance.ShowAreaInfoForTexts(textsForTake);
        //GameSceneCanvas.Instance.ShowAreaInfoForTextsButtons(textsButtonsForTake);
        //_isOrderBoxOpenAvailable = false;
    }
    public void ShowOutline(bool active)
    {
        return;    
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

    public string[] GetInterectableButtons()
    {
        return null;

    }

    public void Move()
    {
        
    }

    public void Open()
    {
        
    }

    public Enums.PlayerStateType GetStateType()
    {
        return Enums.PlayerStateType.Free;

    }

    public Enums.FoodIngredientType GetIngredientType()
    {
        return _shopItemDataIngredientType;
    }

    public GameObject GetIngredientObject()
    {
        if (_ingredientList.Count > 0)
        {
            var ingredientObject = _ingredientList[_ingredientList.Count-1];
            _ingredientList.Remove(ingredientObject);
            return ingredientObject;
        }

        return null;
    }
}

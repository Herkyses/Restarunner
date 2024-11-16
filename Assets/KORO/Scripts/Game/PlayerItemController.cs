using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    [SerializeField] private Transform _itemsParent;
    [SerializeField] private List<string> _itemTexts;
    [SerializeField] private List<Sprite> _itemSprites;
    [SerializeField] private SingleItemInfo _singleItemInfoPf;
    // Start is called before the first frame update
    void Start()
    {
        Utilities.DeleteTransformchilds(_itemsParent);
        CreateItems();
    }

    public void CreateItems()
    {
        for (int i = 0; i < _itemSprites.Count; i++)
        {
            var singleItemInfo = Instantiate(_singleItemInfoPf, transform);
            singleItemInfo.Initiliaze(_itemTexts[i],_itemSprites[i]);
        }
    }
}

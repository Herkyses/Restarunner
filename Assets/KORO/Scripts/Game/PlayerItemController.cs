using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    [SerializeField] private Transform _itemsParent;
    [SerializeField] private List<TextMeshProUGUI> _itemTexts;
    [SerializeField] private List<Sprite> _itemSprites;
    [SerializeField] private SingleItemInfo _singleItemInfoPf;
    // Start is called before the first frame update
    void Start()
    {
        Utilities.DeleteTransformchilds(_itemsParent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

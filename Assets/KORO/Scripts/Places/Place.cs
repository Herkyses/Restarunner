using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Place : MonoBehaviour
{
    [SerializeField] private PlaceInfo _placeInfo;
    [SerializeField] private Canvas _placeCanvas;
    [SerializeField] private TextMeshProUGUI _placePriceText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPlacePrice()
    {
        _placeCanvas.gameObject.SetActive(true);
        _placePriceText.text = _placeInfo.Price.ToString();
    }
}

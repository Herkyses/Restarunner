using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleItemInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private Image _buttonInfoImage;

    public void Initiliaze(string ButtonText, Sprite ButtonImage)
    {
        _buttonText.text = ButtonText;
        _buttonInfoImage.sprite = ButtonImage;
    }
}

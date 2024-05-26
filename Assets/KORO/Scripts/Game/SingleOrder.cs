using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SingleOrder : MonoBehaviour
{
    public OrderData OrderData;
    public Enums.OrderType OrderType;
    public TextMeshProUGUI OrderText;

    public void Initialize()
    {
        OrderText.text = OrderType.ToString();
    }
}

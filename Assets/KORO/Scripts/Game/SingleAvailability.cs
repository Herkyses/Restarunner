using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleAvailability : MonoBehaviour
{
    public int TableNumber;
    public Image TableImageBg;
    public TextMeshProUGUI AvailabilityText;


    public void SingleAvailabilityInitialize(int tableNumber)
    {
        AvailabilityText.text = tableNumber.ToString();
    }
}

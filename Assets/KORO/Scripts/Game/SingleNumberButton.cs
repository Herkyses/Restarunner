using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SingleNumberButton : MonoBehaviour
{
    public int NumberValue;
    public TextMeshProUGUI NumberValueText;

    public void Initiliaze(int number)
    {
        NumberValue = number;
        NumberValueText.text = (NumberValue + 1).ToString();
    }
}

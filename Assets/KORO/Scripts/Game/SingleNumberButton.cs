using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SingleNumberButton : MonoBehaviour
{
    public int NumberValue;
    public TextMeshProUGUI NumberValueText;
    public Enums.BillButtonType ButtonType;

    public void Initiliaze(int number)
    {
        NumberValue = number+1;
        NumberValueText.text = (NumberValue).ToString();
    }

    public void ButtonPressed()
    {
        switch (ButtonType)
        {
            case Enums.BillButtonType.Clear:
                CheckOrderBillsPanel.Instance.ClearInput();
                break;
            case Enums.BillButtonType.Delete:
                CheckOrderBillsPanel.Instance.Backspace();
                break;
            case Enums.BillButtonType.Number:
                CheckOrderBillsPanel.Instance.OnNumberButtonClick(NumberValue.ToString());
                break;
            case Enums.BillButtonType.Zero:
                break;
        }
    }

    public void ClearButtonPressed(Enums.BillButtonType billButtonType)
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class SingleNumberButton : MonoBehaviour
{
    public int NumberValue;
    public TextMeshProUGUI NumberValueText;
    public Enums.BillButtonType ButtonType;
    [Inject] private CheckOrderBillsPanel _checkOrderBillsPanel;


    public void Initiliaze(int number)
    {
        switch (ButtonType)
        {
            case Enums.BillButtonType.Clear:
                NumberValueText.text = "C";
                break;
            case Enums.BillButtonType.Delete:
                NumberValueText.text = "Del";

                break;
            case Enums.BillButtonType.Number:
                NumberValue = number+1;
                NumberValueText.text = (NumberValue).ToString();
                break;
            case Enums.BillButtonType.Zero:
                number = -1;
                NumberValue = number+1;
                NumberValueText.text = (NumberValue).ToString();
                break;
            default:
                NumberValue = number+1;
                NumberValueText.text = (NumberValue).ToString();
                break;
        }
        
    }

    public void ButtonPressed()
    {
        switch (ButtonType)
        {
            case Enums.BillButtonType.Clear:
                _checkOrderBillsPanel.ClearInput();
                break;
            case Enums.BillButtonType.Delete:
                _checkOrderBillsPanel.Backspace();
                break;
            case Enums.BillButtonType.Number:
                _checkOrderBillsPanel.OnNumberButtonClick(NumberValue.ToString());
                break;
            case Enums.BillButtonType.Zero:
                _checkOrderBillsPanel.OnNumberButtonClick(0.ToString());

                break;
        }
    }

    public void ClearButtonPressed(Enums.BillButtonType billButtonType)
    {
        
    }
}

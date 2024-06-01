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
    public Table OwnerTable;
    public bool IsSingleAvailabilityButtonPressed;


    public void SingleAvailabilityInitialize(int tableNumber, Table ownerTable)
    {
        AvailabilityText.text = tableNumber.ToString();
        OwnerTable = ownerTable;
    }

    public void SingleAvailabilityButtonPressed()
    {
        if (!IsSingleAvailabilityButtonPressed)
        {
            IsSingleAvailabilityButtonPressed = true;
            AIWaitStateController.Instance.AISetTablePos(TableAvailablePanel.Instance.SelectedCustomerIndex,OwnerTable.transform);
            TableImageBg.color = Color.red;
        }
        
        
    }
}

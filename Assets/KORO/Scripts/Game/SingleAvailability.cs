using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleAvailability : MonoBehaviour
{
    public int TableNumber;
    public int CustomerCount;
    public Image TableImageBg;
    public TextMeshProUGUI AvailabilityText;
    public TextMeshProUGUI CustomerCountText;
    public Table OwnerTable;
    public bool IsSingleAvailabilityButtonPressed;


    public void SingleAvailabilityInitialize(int tableNumber, Table ownerTable)
    {
        AvailabilityText.text = (tableNumber+1).ToString();
        OwnerTable = ownerTable;
        TableNumber = ownerTable.TableNumber;
        CustomerCountText.text = OwnerTable.TableCapacity.ToString();
    }

    public void SingleAvailabilityButtonPressed()
    {
        var availabilityPanel = ControllerManager.Instance.TableAvailablePanel;
        if (OwnerTable.TableCapacity >= availabilityPanel.GetCustomerCount(availabilityPanel.SelectedCustomerIndex)+1)
        {
            if (availabilityPanel.IsCustomerSelected)
            {
                if (!IsSingleAvailabilityButtonPressed)
                {
                    availabilityPanel.IsCustomerSelected = false;
                
                    IsSingleAvailabilityButtonPressed = true;
                    TableImageBg.color = Color.red;
                    OwnerTable.IsTableAvailable = false;
                    /*CustomerCount++;
                    if (CustomerCount >= OwnerTable.TableCapacity)
                    {
                        IsSingleAvailabilityButtonPressed = true;
                        TableImageBg.color = Color.red;
                    }*/
                    AIWaitStateController.Instance.AISetTablePos(availabilityPanel.SelectedCustomerIndex,OwnerTable);
                }
            }
        }
        
        
    }

    public void CheckTable()
    {
        if (CustomerCount >= OwnerTable.TableCapacity)
        {
            IsSingleAvailabilityButtonPressed = true;
            TableImageBg.color = Color.red;
        }
        else
        {
            IsSingleAvailabilityButtonPressed = false;
            TableImageBg.color = Color.white;
        }
    }
    public void SingleAvailabilityButtonDeactivate()
    {
        IsSingleAvailabilityButtonPressed = false;
        //AIWaitStateController.Instance.AISetTablePos(TableAvailablePanel.Instance.SelectedCustomerIndex,OwnerTable.transform);
        TableImageBg.color = Color.white;
    }
}

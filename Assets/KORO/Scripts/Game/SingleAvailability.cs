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
        AvailabilityText.text = tableNumber.ToString();
        OwnerTable = ownerTable;
        TableNumber = ownerTable.TableNumber;
        CustomerCountText.text = OwnerTable.TableCapacity.ToString();
    }

    public void SingleAvailabilityButtonPressed()
    {
        if (OwnerTable.TableCapacity >= TableAvailablePanel.Instance.GetCustomerCount(TableAvailablePanel.Instance.SelectedCustomerIndex)+1)
        {
            if (TableAvailablePanel.Instance.IsCustomerSelected)
            {
                if (!IsSingleAvailabilityButtonPressed)
                {
                    TableAvailablePanel.Instance.IsCustomerSelected = false;
                
                    IsSingleAvailabilityButtonPressed = true;
                    TableImageBg.color = Color.red;
                    /*CustomerCount++;
                    if (CustomerCount >= OwnerTable.TableCapacity)
                    {
                        IsSingleAvailabilityButtonPressed = true;
                        TableImageBg.color = Color.red;
                    }*/
                    AIWaitStateController.Instance.AISetTablePos(TableAvailablePanel.Instance.SelectedCustomerIndex,OwnerTable);
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

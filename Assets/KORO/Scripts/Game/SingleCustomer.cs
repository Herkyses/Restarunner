using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SingleCustomer : MonoBehaviour
{
    public int aiIndex;
    public TextMeshProUGUI aiIndexText;
    public TextMeshProUGUI FriendCount;
    public int FriendCountInt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitializeSingleCustom(int customerIndex,int friendCount)
    {
        aiIndex = customerIndex;
        //aiIndexText.text = aiIndex.ToString();
        aiIndexText.text = "+" + friendCount.ToString();
        FriendCountInt = friendCount;
        //FriendCount.text = friendCount.ToString();
    }

    public void SetCustomerId()
    {
        ControllerManager.Instance.TableAvailablePanel.SelectedCustomerIndex = aiIndex;
        ControllerManager.Instance.TableAvailablePanel.SetCustomerSelected(aiIndex);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

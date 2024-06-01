using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SingleCustomer : MonoBehaviour
{
    public int aiIndex;
    public TextMeshProUGUI aiIndexText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitializeSingleCustom(int customerIndex)
    {
        aiIndex = customerIndex;
        aiIndexText.text = aiIndex.ToString();
    }

    public void SetCustomerId()
    {
        TableAvailablePanel.Instance.SelectedCustomerIndex = aiIndex;
        TableAvailablePanel.Instance.SetCustomerSelected(aiIndex);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

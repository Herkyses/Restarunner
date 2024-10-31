using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SingleBill : MonoBehaviour
{
    public int TableNumber;
    public float TableOrderBill;
    public Color DefaultColor;
    public TextMeshProUGUI BillText;
    public TextMeshProUGUI TableNumberText;
    // Start is called before the first frame update
    void Start()
    {
        DefaultColor = GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(int tableNumber,float totalBill)
    {
        TableNumber = tableNumber;
        TableOrderBill = totalBill;
        BillText.text = TableOrderBill.ToString();
        TableNumberText.text = (TableNumber+1).ToString();
    }

    public void SingleBillSelected()
    {
        ControllerManager.Instance._checkOrderBillsPanel.SelectedTable = TableNumber;
        ControllerManager.Instance._checkOrderBillsPanel.SelectedOrderBillforInducator(TableNumber);
    }

    public void SelectInducator()
    {
        GetComponent<Image>().color = Color.green;
    }
    public void SelectedDefaultColor()
    {
        GetComponent<Image>().color = DefaultColor;
    }
}

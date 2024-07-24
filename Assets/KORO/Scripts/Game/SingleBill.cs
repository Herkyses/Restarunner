using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleBill : MonoBehaviour
{
    public int TableNumber;
    public float TableOrderBill;
    public Color DefaultColor;
    public TextMeshProUGUI BillText;
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
    }

    public void SingleBillSelected()
    {
        CheckOrderBillsPanel.Instance.SelectedTable = TableNumber;
        CheckOrderBillsPanel.Instance.SelectedOrderBillforInducator(TableNumber);
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

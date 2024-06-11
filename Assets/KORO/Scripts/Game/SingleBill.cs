using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SingleBill : MonoBehaviour
{
    public int TableNumber;
    public float TableOrderBill;

    public TextMeshProUGUI BillText;
    // Start is called before the first frame update
    void Start()
    {
        
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CheckOrderBillsPanel : MonoBehaviour
{
    [SerializeField] private Transform _panel;
    [SerializeField] private Transform _billParent;
    [SerializeField] private Transform _numberButtonParent;
    [SerializeField] private SingleBill _singleBill;
    public List<SingleBill> BillList;
    public static CheckOrderBillsPanel Instance;
    public int SelectedTable;

    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }    
    }
    public void Initialize()
    {
        Utilities.DeleteTransformchilds(_billParent);
        CreateBills();
        CreateButtonNumbers();
    }

    public void UpdatePanel(int tableNumber,float totalBill)
    {
        for (int i = 0; i < BillList.Count; i++)
        {
            if (BillList[i].TableNumber == tableNumber)
            {
                BillList[i].TableOrderBill = totalBill;
                BillList[i].Initialize(tableNumber,totalBill);
                return;
            }
        }
        var availabilityArray = TableAvailablePanel.Instance._tablesParent.GetComponentsInChildren<Table>();

        var bill = Instantiate(_singleBill, _billParent);
        bill.Initialize(availabilityArray[tableNumber].TableNumber,availabilityArray[tableNumber].TotalBills);
        BillList.Add(bill);
    }

    public void CreateBills()
    {
        var availabilityArray = TableAvailablePanel.Instance._tablesParent.GetComponentsInChildren<Table>();

        for (int i = 0; i < availabilityArray.Length; i++)
        {
            var bill = Instantiate(_singleBill, _billParent);
            bill.Initialize(availabilityArray[i].TableNumber,availabilityArray[i].TotalBills);
            BillList.Add(bill);
        }
    }

    public void UpdateBillList(int index)
    {
        for (int i = 0; i < BillList.Count; i++)
        {
            if (index == BillList[i].TableNumber)
            {
                BillList[i].Initialize(BillList[i].TableNumber,0);
            }
        }
    }

    public void CreateButtonNumbers()
    {
        var buttons = _numberButtonParent.GetComponentsInChildren<SingleNumberButton>();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].Initiliaze(i);
        }
    }

    public void CreateTableBill()
    {
        if (TableController.Instance.TableSets[SelectedTable].table.CheckAllCustomerFinishedFood())
        {
            BillTable.Instance.CreateTableBill(TableController.Instance.TableSets[SelectedTable].table);
            DeActiveBillsPanel();
        }
        
    }
    
    public void ActiveBillsPanel()
    {
        GameSceneCanvas.Instance.CanMove = false;
        GameSceneCanvas.IsCursorVisible?.Invoke(true);
        _panel.gameObject.SetActive(true);
    }
    public void DeActiveBillsPanel()
    {
        GameSceneCanvas.Instance.CanMove = true;
        GameSceneCanvas.IsCursorVisible?.Invoke(false);
        _panel.gameObject.SetActive(false);

    }
   

    public void SelectedOrderBillforInducator(int tableNumber)
    {
        for (int i = 0; i < BillList.Count; i++)
        {
            if (tableNumber == BillList[i].TableNumber)
            {
                BillList[i].SelectInducator();
            }
            else
            {
                BillList[i].SelectedDefaultColor();

            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class CheckOrderBillsPanel : MonoBehaviour,IPanel
{
    [SerializeField] private Transform _panel;
    [SerializeField] private Transform _billParent;
    [SerializeField] private Transform _numberButtonParent;
    [SerializeField] private SingleBill _singleBill;
    private GameSceneCanvas _gameSceneCanvas;
    
    public TMP_InputField billInputField; // Fatura değerini gösterecek alan
    public Button[] numberButtons;        // 0-9 arası sayı butonları
    public Button clearButton;            // Sil butonu
    public Button confirmButton;          // Onayla butonu
    
    
    public List<SingleBill> BillList;
    public int SelectedTable;

    public void Show() => ActiveBillsPanel();
    public void Hide() => gameObject.SetActive(false);
   
    private void Start()
    {
        _gameSceneCanvas = GameSceneCanvas.Instance;
        billInputField.text = String.Empty;
        ControllerManager.Instance.RegisterPanel("CheckOrderBillsPanel", this);
       
    }
    // Sayı butonuna tıklandığında çağrılacak fonksiyon
    public void OnNumberButtonClick(string number)
    {
        if (billInputField.text != "0")
        {
            billInputField.text += number;
        }
        else
        {
            billInputField.text = number;
        }
    }

    // Girdi alanını temizleme
    public void ClearInput()
    {
        billInputField.text = "";
    }
    
    public void Backspace()
    {
        if (billInputField.text.Length > 0)
        {
            // Mevcut metnin son karakterini siler
            billInputField.text = billInputField.text.Substring(0, billInputField.text.Length - 1);
        }
    }
    
    public int ValidateBillValue(string billValue)
    {
        if (int.TryParse(billValue, out int result) && result > 0)
        {
            return result;
        }
        return -1;
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
        var availabilityArray = ControllerManager.Instance.TableAvailablePanel._tablesParent.GetComponentsInChildren<Table>();

        var bill = Instantiate(_singleBill, _billParent);
        bill.Initialize(availabilityArray[tableNumber].TableNumber,availabilityArray[tableNumber].TotalBills);
        BillList.Add(bill);
    }

    public void CreateBills()
    {
        var availabilityArray = ControllerManager.Instance.TableAvailablePanel._tablesParent.GetComponentsInChildren<Table>();

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
            buttons[i].GetComponent<SingleNumberButton>().Initiliaze(i);
        }
    }

    public void CreateTableBill()
    {
        Debug.Log("createBill");
        if (ControllerManager.Instance.Tablecontroller.TableSets[SelectedTable].table.CheckAllCustomerFinishedFood())
        {
            BillTable.Instance.CreateTableBill(ControllerManager.Instance.Tablecontroller.TableSets[SelectedTable].table,ValidateBillValue(billInputField.text));
            DeActiveBillsPanel();
        }
        
    }
    
    public void ActiveBillsPanel()
    {
        _gameSceneCanvas.CanMove = false;
        GameSceneCanvas.IsCursorVisible?.Invoke(true);
        _panel.gameObject.SetActive(true);
    }
    public void DeActiveBillsPanel()
    {
        _gameSceneCanvas.CanMove = true;
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

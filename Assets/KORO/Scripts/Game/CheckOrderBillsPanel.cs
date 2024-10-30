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
    
    public TMP_InputField billInputField; // Fatura değerini gösterecek alan
    public Button[] numberButtons;        // 0-9 arası sayı butonları
    public Button clearButton;            // Sil butonu
    public Button confirmButton;          // Onayla butonu
    
    
    public List<SingleBill> BillList;
    //public static CheckOrderBillsPanel Instance;
    public int SelectedTable;

    public void Show() => ActiveBillsPanel();
    public void Hide() => gameObject.SetActive(false);
    /*private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }    
    }*/
    private void Start()
    {
        foreach (Button button in numberButtons)
        {
            //button.onClick.AddListener(() => OnNumberButtonClick(button.GetComponentInChildren<TMP_Text>().text));
        }
        billInputField.text = String.Empty;
        PanelManager.Instance.RegisterPanel("CheckOrderBillsPanel", this);
        //clearButton.onClick.AddListener(ClearInput);
        //confirmButton.onClick.AddListener(ConfirmBill);
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

    // Fatura doğrulama
    void ConfirmBill()
    {
        if (ValidateBill(billInputField.text))
        {
            Debug.Log("Fatura kabul edildi: " + billInputField.text);
            // İşlemleri burada yapabilirsiniz
        }
        else
        {
            Debug.Log("Geçersiz fatura değeri!");
            // Kullanıcıya hata mesajı gösterebilirsiniz
        }
    }
    // Son karakteri silme (backspace)
    public void Backspace()
    {
        if (billInputField.text.Length > 0)
        {
            // Mevcut metnin son karakterini siler
            billInputField.text = billInputField.text.Substring(0, billInputField.text.Length - 1);
        }
    }

    // Fatura değerinin doğru olup olmadığını kontrol edin (Örneğin pozitif sayı mı)
    bool ValidateBill(string billValue)
    {
        if (int.TryParse(billValue, out int result) && result > 0)
        {
            return true;
        }
        return false;
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
            buttons[i].GetComponent<SingleNumberButton>().Initiliaze(i);
        }
    }

    public void CreateTableBill()
    {
        Debug.Log("createBill");
        if (TableController.Instance.TableSets[SelectedTable].table.CheckAllCustomerFinishedFood())
        {
            BillTable.Instance.CreateTableBill(TableController.Instance.TableSets[SelectedTable].table,ValidateBillValue(billInputField.text));
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

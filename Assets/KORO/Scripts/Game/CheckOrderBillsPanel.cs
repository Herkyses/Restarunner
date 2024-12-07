using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class CheckOrderBillsPanel : MonoBehaviour,IPanel
{
    [SerializeField] private Transform _panel;
    [SerializeField] private Transform _payedPanel;
    [SerializeField] private Transform _billParent;
    [SerializeField] private Transform _numberButtonParent;
    [SerializeField] private SingleBill _singleBill;
    private Tween _payedPanelMove;

    private GameSceneCanvas _gameSceneCanvas;
    private TableController _tableController;
    private Table _openedTable;
    
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
        _tableController = ControllerManager.Instance.Tablecontroller;

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
        if (_tableController.TableSets[SelectedTable].table.CheckAllCustomerFinishedFood())
        {
            //BillTable.Instance.CreateTableBill(_tableController.TableSets[SelectedTable].table,ValidateBillValue(billInputField.text));
            BillTable.Instance.CreateTableBill(_tableController.TableSets[SelectedTable].table,(int)_tableController.TableSets[SelectedTable].table.TotalBills);
            DeActiveBillsPanel();
        }
        
    }
    
    public void ActiveBillsPanel()
    {
        _gameSceneCanvas.CanMove = false;
        GameSceneCanvas.IsCursorVisible?.Invoke(true);
        _panel.gameObject.SetActive(true);
    }

    public void ActivePayedPanel(Table table)
    {
        
        OpenPayedPanel();
        _openedTable = table;
    }

    public void OpenPayedPanel()
    {
        if (_payedPanel.gameObject.activeSelf)
        {
            //DeActivePlacePanel();
            if (_payedPanelMove != null)
            {
                _payedPanelMove.Kill();
            }
            var panelRect = _payedPanel.GetComponent<RectTransform>();
            _payedPanelMove = panelRect.DOLocalMoveY( - 1200f, 0.3f).OnComplete(DeactiveCanmoveCursor);
        }
        else
        {
            ActiveCanMoveCursor();
            var panelRect = _payedPanel.GetComponent<RectTransform>();
            panelRect.anchoredPosition += Vector2.down*600f; 
            if (_payedPanelMove != null)
            {
                _payedPanelMove.Kill();
            }
            _payedPanelMove = panelRect.DOLocalMoveY(0f, 0.3f);


        }
    }

    public void ActiveCanMoveCursor()
    {
        _payedPanel.gameObject.SetActive(true);
        _gameSceneCanvas.CanMove = false;
        GameSceneCanvas.IsCursorVisible?.Invoke(true);
    }

    public void PayedButtonPressed()
    {
        if (_openedTable && ValidateBillValue(billInputField.text) == _openedTable.TotalBills)
        {
            _openedTable.AIPayed();
        }
    }
    public void DeActivePayedPanel()
    {
        OpenPayedPanel();
        
        //_openedTable = null;
    }

    public void DeactiveCanmoveCursor()
    {
        _payedPanel.gameObject.SetActive(false);
        _gameSceneCanvas.CanMove = true;
        GameSceneCanvas.IsCursorVisible?.Invoke(false);
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

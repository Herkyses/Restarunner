using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CheckOrderBillsPanel : MonoBehaviour
{
    [SerializeField] private Transform _panel;
    [SerializeField] private Transform _billParent;
    [SerializeField] private SingleBill _singleBill;
    public List<SingleBill> BillList;
    public static CheckOrderBillsPanel Instance;

    
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
        DeleteChilds(_billParent);
        CreateBills();
    }

    public void UpdatePanel(int tableNumber,float totalBill)
    {
        for (int i = 0; i < BillList.Count; i++)
        {
            if (BillList[i].TableNumber == tableNumber)
            {
                BillList[i].TableOrderBill = totalBill;
                BillList[i].Initialize(tableNumber,totalBill);
            }
        }
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActiveBillsPanel()
    {
        GameSceneCanvas.Instance.CanMove = false;
        _panel.gameObject.SetActive(true);
    }
    public void DeActiveBillsPanel()
    {
        GameSceneCanvas.Instance.CanMove = true;
        _panel.gameObject.SetActive(false);

    }
    public void DeleteChilds(Transform parent)
    {
        var orderArray = parent.GetComponentsInChildren<SingleBill>();
        if (orderArray.Length > 0)
        {
            for (int i = 0; i < orderArray.Length; i++)
            {
                Destroy(orderArray[i].gameObject);
            }
        }
    }
}

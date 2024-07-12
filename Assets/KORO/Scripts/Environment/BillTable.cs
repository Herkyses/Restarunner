using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillTable : MonoBehaviour,IInterectableObject
{
    
    public static BillTable Instance;

    [SerializeField] private Transform _tableBillParent;
    [SerializeField] private TableBill _tableBillPf;
    [SerializeField] private TableBill _tableBillTemp;

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateTableBill(Table ownerTable)
    {
        DeleteChilds();
        if (!_tableBillTemp)
        {
            _tableBillTemp = Instantiate(_tableBillPf, _tableBillParent);
        }
        else
        {
            _tableBillTemp.gameObject.SetActive(true);
        }
        _tableBillTemp.transform.localPosition = Vector3.zero;
        var zort = _tableBillParent.transform.rotation.eulerAngles;
        _tableBillTemp.transform.localRotation = Quaternion.Euler(zort);
        _tableBillTemp.BillValue = ownerTable.TotalBills;
        _tableBillTemp.OwnerTable = ownerTable;
    }
    public void UpdateTableBill(TableBill tableBill)
    {
        tableBill.gameObject.SetActive(false);
        tableBill.transform.SetParent(_tableBillParent);
        tableBill.transform.localPosition = Vector3.zero;
    }

    public void InterectableObjectRun()
    {
        CheckOrderBillsPanel.Instance.ActiveBillsPanel();
    }

    public void ShowOutline(bool active)
    {
        
    }

    public Outline GetOutlineComponent()
    {
        return null;
    }

    public string GetInterectableText()
    {
        return "Check Order Bills";
    }

    public void Move()
    {
        
    }
    public void DeleteChilds()
    {
        var orderArray = _tableBillParent.GetComponentsInChildren<TableBill>();
        if (orderArray.Length > 0)
        {
            for (int i = 0; i < orderArray.Length; i++)
            {
                Destroy(orderArray[i].gameObject);
            }
        }
    }
}

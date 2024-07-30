using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillTable : MonoBehaviour,IInterectableObject
{
    
    public static BillTable Instance;

    
    [SerializeField] private string[] texts = new [] {"Check Order "};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    [SerializeField] private Transform _tableBillParent;
    [SerializeField] private TableBill _tableBillPf;
    [SerializeField] private TableBill _tableBillTemp;
    [SerializeField] private Outline _tableBillOutline;

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
        texts = new []{"Check Order Bills"};
        textsButtons = new []{"E"};
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateTableBill(Table ownerTable)
    {
        DeleteChilds();
        /*if (!_tableBillTemp)
        {
            _tableBillTemp = Instantiate(_tableBillPf, _tableBillParent);
        }
        else
        {
            _tableBillTemp.gameObject.SetActive(true);
        }*/

        _tableBillTemp = PoolManager.Instance.GetFromPoolForOrderBill().GetComponent<TableBill>();
        _tableBillTemp.transform.SetParent(_tableBillParent);
        _tableBillTemp.transform.localPosition = Vector3.zero;
        var zort = _tableBillParent.transform.rotation.eulerAngles;
        _tableBillTemp.transform.localRotation = Quaternion.Euler(zort);
        _tableBillTemp.BillValue = ownerTable.TotalBills;
        _tableBillTemp.OwnerTable = ownerTable;
    }
    public void UpdateTableBill(TableBill tableBill)
    {
        PoolManager.Instance.ReturnToPoolForOrderBill(tableBill.gameObject);
        /*tableBill.gameObject.SetActive(false);
        tableBill.transform.SetParent(_tableBillParent);
        tableBill.transform.localPosition = Vector3.zero;*/
    }

    public void InterectableObjectRun()
    {
        CheckOrderBillsPanel.Instance.ActiveBillsPanel();
    }

    public void ShowOutline(bool active)
    {
        _tableBillOutline.enabled = active;
    }

    public Outline GetOutlineComponent()
    {
        return _tableBillOutline;
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
    public void Open()
    {
        
    }
    public string[] GetInterectableTexts()
    {
        return texts;
    }
    public string[] GetInterectableButtons()
    {
        return textsButtons;
    }
    public Enums.PlayerStateType GetStateType()
    {
        return Enums.PlayerStateType.Free;
    }
}

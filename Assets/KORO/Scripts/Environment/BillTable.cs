using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BillTable : MonoBehaviour,IInterectableObject
{
    
    public static BillTable Instance;

    
    [SerializeField] private string[] texts = new [] {"Check Order "};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    [SerializeField] private Transform _tableBillParent;
    [SerializeField] private TableBill _tableBillPf;
    [SerializeField] private TableBill _tableBillTemp;
    [SerializeField] private Outline _tableBillOutline;
    [Inject] private CheckOrderBillsPanel _checkOrderBillsPanel;

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
        texts = new []{"Key_Check_Bills"};
        textsButtons = new []{"E"};
    }

   

    public void CreateTableBill(Table ownerTable,int billValue)
    {
        
        Utilities.DeleteTransformchilds(_tableBillParent);
     

        _tableBillTemp = PoolManager.Instance.GetFromPoolForOrderBill().GetComponent<TableBill>();
        _tableBillTemp.transform.SetParent(_tableBillParent);
        _tableBillTemp.transform.localPosition = Vector3.zero;
        var zort = _tableBillParent.transform.rotation.eulerAngles;
        _tableBillTemp.transform.localRotation = Quaternion.Euler(zort);
        _tableBillTemp.Initiliaze(ownerTable,billValue);
        
    }
    public void UpdateTableBill(TableBill tableBill)
    {
        PoolManager.Instance.ReturnToPoolForOrderBill(tableBill.gameObject);

    }

    public void InterectableObjectRun()
    {
        _checkOrderBillsPanel.ActiveBillsPanel();
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

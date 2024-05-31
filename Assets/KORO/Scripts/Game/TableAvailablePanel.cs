using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableAvailablePanel : MonoBehaviour
{
    [SerializeField] private Transform _availabilityPanel;
    [SerializeField] private Transform _tablesParent;
    [SerializeField] private Transform _customerParent;
    [SerializeField] private Transform _contentParent;
    
    [SerializeField] private SingleAvailability _singleAvailabilityPf;
    [SerializeField] private SingleCustomer _customerPf;
    [SerializeField] private List<SingleAvailability> _availabilityList;
    [SerializeField] private List<SingleCustomer> _customerList;

    public static TableAvailablePanel Instance;
    // Start is called before the first frame update
    
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
    void Start()
    {
        InitializeAvailabilityPanel();
        DeleteCustomerChilds();

    }

    public void InitializeAvailabilityPanel()
    {
        _availabilityList.Clear();
        DeleteChilds();
        var availabilityArray = _tablesParent.GetComponentsInChildren<Table>();

        for (int i = 0; i < availabilityArray.Length; i++)
        {
            var singleAvailability = Instantiate(_singleAvailabilityPf, _contentParent);
            _availabilityList.Add(singleAvailability);
            singleAvailability.SingleAvailabilityInitialize(availabilityArray[i].TableNumber);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            _availabilityPanel.gameObject.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.G))
        {
            _availabilityPanel.gameObject.SetActive(false);
        }
    }

    public void ActiveAbilityPanel()
    {
        GameSceneCanvas.Instance.CanMove = false;
        _availabilityPanel.gameObject.SetActive(true);

    }
    public void DeActiveAbilityPanel()
    {
        GameSceneCanvas.Instance.CanMove = true;
        _availabilityPanel.gameObject.SetActive(false);

    }

    public void SetCustomerList()
    {
        var customer = Instantiate(_customerPf, _customerParent);
        _customerList.Add(customer);
    }
    public void DeleteChilds()
    {
        var orderArray = _tablesParent.GetComponentsInChildren<SingleAvailability>();
        if (orderArray.Length > 0)
        {
            for (int i = 0; i < orderArray.Length; i++)
            {
                Destroy(orderArray[i].gameObject);
            }
        }
    }
    public void DeleteCustomerChilds()
    {
        var orderArray = _tablesParent.GetComponentsInChildren<SingleCustomer>();
        if (orderArray.Length > 0)
        {
            for (int i = 0; i < orderArray.Length; i++)
            {
                Destroy(orderArray[i].gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableAvailablePanel : MonoBehaviour
{
    [SerializeField] private Transform _availabilityPanel;
    [SerializeField] private Transform _tablesParent;
    [SerializeField] private Transform _contentParent;
    
    [SerializeField] private SingleAvailability _singleAvailabilityPf;
    [SerializeField] private List<SingleAvailability> _availabilityList;
    // Start is called before the first frame update
    void Start()
    {
        InitializeAvailabilityPanel();
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
}

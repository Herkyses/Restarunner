using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TableAvailablePanel : MonoBehaviour
{
    [Inject] public TableController Tablecontroller;
    
    [SerializeField] private Transform _availabilityPanel;
    public Transform _tablesParent;
    [SerializeField] private Transform _customerParent;
    [SerializeField] private Transform _contentParent;
    
    // MINIMAP
    [Header("MINIMAP")]
    public GameObject referenceObject; // Örneğin oyuncu
    public RectTransform minimapBounds; // Minimap'in UI alanı
    public float visibilityRadius = 20f; // İkonun görünür olması için mesafe limiti
    private List<GameObject> activeIcons = new List<GameObject>(); // Aktif ikonları tutar
    public Vector2 mapScale = new Vector2(50, 50); // Dünya -> minimap ölçeği
    public List<Transform> tables; // Masaların Transform listesi
    public GameObject tableIconPrefab; // Masa ikonu prefab
    
    
    [SerializeField] private SingleAvailability _singleAvailabilityPf;
    [SerializeField] private SingleCustomer _customerPf;
    public List<SingleAvailability> _availabilityList;
    [SerializeField] private List<SingleCustomer> _customerList;
    
    public int SelectedCustomerIndex;
    public bool IsCustomerSelected;

    //public static TableAvailablePanel Instance;
    // Start is called before the first frame update
    
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
    

    public void Initialize()
    {
        InitializeAvailabilityPanel();
        Utilities.DeleteTransformchilds(_customerParent);
        referenceObject = RestaurantCanvasManager.Instance.referanceObject;
        //CheckOrderBillsPanel.Instance.Initialize();
    }

    public void InitializeAvailabilityPanel()
    {
        _availabilityList.Clear();
        Utilities.DeleteTransformchilds(_contentParent);
        var availabilityArray = _tablesParent.GetComponentsInChildren<Table>();

        for (int i = 0; i < availabilityArray.Length; i++)
        {
            var singleAvailability = Instantiate(_singleAvailabilityPf, _contentParent);
            singleAvailability.CustomerCount = availabilityArray[i].CustomerCount;
            _availabilityList.Add(singleAvailability);
            singleAvailability.SingleAvailabilityInitialize(availabilityArray[i].TableNumber , availabilityArray[i]);
        }
    }

    public void AddNewTable(Table newTable)
    {
        var singleAvailability = Instantiate(_singleAvailabilityPf, _contentParent);
        singleAvailability.CustomerCount = newTable.CustomerCount;
        _availabilityList.Add(singleAvailability);
        singleAvailability.SingleAvailabilityInitialize(newTable.TableNumber , newTable);
    }

    public void CheckTable(int tableIndex)
    {
        var availabilityArray = _tablesParent.GetComponentsInChildren<Table>();

        for (int i = 0; i < availabilityArray.Length; i++)
        {
            if (i == tableIndex)
            {
                availabilityArray[i].CheckChairAvailable();
            }
        }
    }

    public int GetCustomerCount(int index)
    {
        for (int i = 0; i < _customerList.Count; i++)
        {
            if (index == _customerList[i].aiIndex)
            {
                return _customerList[i].FriendCountInt;
            }
        }

        return -1;
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
        if(Input.GetKeyUp(KeyCode.P))
        {
            UpdateMinimapIcons();
        }
    }

    public void ActiveAbilityPanel()
    {
        GameSceneCanvas.Instance.CanMove = false;
        GameSceneCanvas.IsCursorVisible?.Invoke(true);
        GameManager.IsAnyPanelOpened?.Invoke(true);
        _availabilityPanel.gameObject.SetActive(true);
    }
    public void DeActiveAbilityPanel()
    {
        GameSceneCanvas.Instance.CanMove = true;
        GameSceneCanvas.IsCursorVisible?.Invoke(false);
        _availabilityPanel.gameObject.SetActive(false);
        GameManager.IsAnyPanelOpened?.Invoke(false);
    }

    public void RedAvailability(int tableIndex)
    {
        for (int i = 0; i < _availabilityList.Count; i++)
        {
            if (tableIndex == _availabilityList[i].TableNumber)
            {
                _availabilityList[i].SingleAvailabilityButtonDeactivate();
                _availabilityList[i].CustomerCount--;
            }
        }
    }

    public void SetCustomerList(int AIIndex,int friendCount)
    {
        var customer = Instantiate(_customerPf, _customerParent);
        customer.InitializeSingleCustom(AIIndex,friendCount);
        _customerList.Add(customer);
    }

    
    public void RemoveFromCustomerList(int AIIndex)
    {
        for (int i = 0; i < _customerList.Count; i++)
        {
            if (AIIndex == _customerList[i].aiIndex)
            {
                var zort = _customerList[i];
                _customerList.Remove(zort);
                if (SelectedCustomerIndex == AIIndex)
                {
                    SelectedCustomerIndex = -1;
                    IsCustomerSelected = false;
                }
                Destroy(zort.gameObject);
            }
        }
    }
    

    public void SetCustomerSelected(int customerIndex)
    {
        for (int i = 0; i < _customerList.Count; i++)
        {
            if (customerIndex == _customerList[i].aiIndex)
            {
                _customerList[i].GetComponent<Image>().color = Color.green;
                IsCustomerSelected = true;

            }
            else
            {
                if (_customerList[i])
                {
                    _customerList[i].GetComponent<Image>().color = Color.white;
                }
            }
        }
    }
    private void UpdateMinimapIcons()
    {
        // Aktif ikonları temizle
        foreach (var icon in activeIcons)
        {
            Destroy(icon);
        }
        activeIcons.Clear();

        foreach (TableSet table in Tablecontroller.TableSets)
        {
            // Masanın referans objeye uzaklığını hesapla
            float distance = Vector3.Distance(referenceObject.transform.position, table.gameObject.transform.position);
            Vector3 distanceVector = referenceObject.transform.position - table.gameObject.transform.position;
            if (distance <= visibilityRadius)
            {
                // İkon oluştur ve minimap'e ekle
                GameObject icon = Instantiate(tableIconPrefab, minimapBounds);
                icon.GetComponent<RectTransform>().anchoredPosition = WorldToMinimap(-distanceVector);
                //icon.GetComponent<RectTransform>().anchoredPosition = new Vector2(distanceVector.x,distanceVector.z);
                activeIcons.Add(icon);
            }
        }
    }
    private Vector2 WorldToMinimap(Vector3 worldPosition)
    {
        // Dünya pozisyonunu minimap pozisyonuna çevir
        Vector2 normalizedPosition = new Vector2(worldPosition.x / mapScale.x, worldPosition.z / mapScale.y);
        Vector2 minimapCenter = minimapBounds.sizeDelta / 2; // Minimap merkezine göre ayarla
        Debug.Log("minimapcenter:" + minimapCenter);
        return /*minimapCenter +*/ Vector2.Scale(normalizedPosition, minimapBounds.sizeDelta);
    }
}

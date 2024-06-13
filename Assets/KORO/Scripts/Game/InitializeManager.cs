using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeManager : MonoBehaviour
{
    public static InitializeManager Instance;
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
        TableController.Instance.Initialize();
        TableAvailablePanel.Instance.Initialize();
        CheckOrderBillsPanel.Instance.Initialize();

        AISpawnController.Instance.Initialize();
        OrderPanelController.Instance.Initialize();
        PlacePanelController.Instance.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

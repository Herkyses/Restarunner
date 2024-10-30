using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PanelManager : MonoBehaviour
{
    private readonly Dictionary<string, IPanel> _panels = new Dictionary<string, IPanel>();
    
    [Inject] public CheckOrderBillsPanel _checkOrderBillsPanel;
    [Inject] public OrderPanelController _orderPanelController;

    public static PanelManager Instance;
    
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

    public void RegisterPanel(string panelName, IPanel panel)
    {
        if (!_panels.ContainsKey(panelName))
        {
            _panels.Add(panelName, panel);
        }
    }

    public void ShowPanel(string panelName)
    {
        if (_panels.TryGetValue(panelName, out var panel))
        {
            panel.Show();
        }
    }

    public void HidePanel(string panelName)
    {
        if (_panels.TryGetValue(panelName, out var panel))
        {
            panel.Hide();
        }
    }
}

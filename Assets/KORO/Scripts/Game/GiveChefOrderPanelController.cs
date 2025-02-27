using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GiveChefOrderPanelController : MonoBehaviour
{

    public static Action<List<OrderDataStruct>,bool,WaiterController> OnOrderGivenToChef;
    public static GiveChefOrderPanelController Instance;
    public Transform Panel;
    [SerializeField] private Transform _singlePfParent;
    [SerializeField] private SingleOrder _singlePf;
    [SerializeField] private TextMeshProUGUI SelectedOrderListCountText;

    public List<Orders> OrderList = new List<Orders>();
    public int SelectedOrderListCount;

    public Orders Orders ;

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

    public void GiveChefOrderPanel()
    {
        Orders = OrderList[SelectedOrderListCount];
        var orderStructs = Orders.OrderDataStructs;
        
        if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 7)
        {
            orderStructs = ControllerManager.Instance.Tablecontroller.TableSets[0].table.GetOrderHandler().GetOrders();
        }
        OnOrderGivenToChef?.Invoke(orderStructs,true,null);
        //PlayerOrderController.Instance.OrderList.Remove(Orders);
        OrderList[SelectedOrderListCount].OrderDataStructs.Clear();
        //TODO: check for tutorial
        if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 5)
        {
            DeactivePanel();
        }
        //
    }

    public void OrderListIndexIncrease(bool isIncrease)
    {
        if (ControllerManager.Instance.Tablecontroller.TableSets.Count > 1)
        {
            if (isIncrease)
            {
                if (SelectedOrderListCount < ControllerManager.Instance.Tablecontroller.TableSets.Count-1)
                {
                    SelectedOrderListCount++;
                }
            }
            else
            {
                if (SelectedOrderListCount >= 1)
                {
                    SelectedOrderListCount--;
                }
            }
        }
        

        SelectedOrderListCountText.text = (SelectedOrderListCount+1).ToString();
        InitiliazeGiveChefOrderPanel();
    }

    public void InitiliazeGiveChefOrderPanel()
    {
        Utilities.DeleteTransformchilds(_singlePfParent);
        if (OrderList.Count > 0)
        {
            for (int i = 0; i < OrderList[SelectedOrderListCount].OrderDataStructs.Count; i++)
            {
                var singleOrder = Instantiate(_singlePf, _singlePfParent);
                singleOrder.OrderType = OrderList[SelectedOrderListCount].OrderDataStructs[i].OrderType;
                singleOrder.Initialize();
            }
        }
        
    }

    public void DeactivePanel()
    {
        Panel.gameObject.SetActive(false);
        GameSceneCanvas.Instance.CanMove = true;
        GameSceneCanvas.IsCursorVisible?.Invoke(false);
    }
    public void OrderPanelInitliaze()
    {
        Panel.gameObject.SetActive(true);
        GameSceneCanvas.Instance.CanMove = false;
        GameSceneCanvas.IsCursorVisible?.Invoke(true);
        GameManager.IsAnyPanelOpened?.Invoke(true);
        OrderList = PlayerOrderController.Instance.OrderList;
        SelectedOrderListCount = 0;
        OrderListIndexIncrease(true);
    }
}

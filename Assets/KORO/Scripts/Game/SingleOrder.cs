using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SingleOrder : MonoBehaviour
{
    public OrderData OrderData;
    public Enums.OrderType OrderType;
    public Enums.SingleOrderUIType SingleOrderUIType;
    public float OrderPrice;
    public TextMeshProUGUI OrderText;
    public Image SingleOrderPlusImage;
    public Image SingleOrderMinusImage;
    public Image FoodImage;
    [Inject] private OrderPanelController _orderPanelController; 
    public void Initialize()
    {
        OrderText.text = OrderType.ToString();
        SingleOrderTypeInitialize();
        FoodImage.sprite = GameDataManager.Instance.GetFoodSprite(OrderType);
    }

    public void SingleOrderTypeInitialize()
    {
        if (SingleOrderUIType == Enums.SingleOrderUIType.FoodList)
        {
            SingleOrderPlusImage.gameObject.SetActive(true);
        }
        else if(SingleOrderUIType == Enums.SingleOrderUIType.PlayerOrderList)
        {
            SingleOrderMinusImage.gameObject.SetActive(true);
        }
    }

    public void PlusButtonPressed()
    {
        if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 4)
        {
            if (TableController.Instance.TableSets[0].table.GetOrders()[0].OrderType == OrderType && PlayerOrderController.Instance.OrderList[0].OrderDataStructs.Count == 0)
            {
                PanelManager.Instance._orderPanelController.PlayerOrderInventory(this);
                TutorialManager.Instance.SetTutorialInfo(7);
                return;
            }
            else
            {
                return;
            }
        }
        PanelManager.Instance._orderPanelController.PlayerOrderInventory(this);
    }
    public void MinusButtonPressed()
    {
        PanelManager.Instance._orderPanelController.RemoveOrderFromLPayer(this);

    }
}

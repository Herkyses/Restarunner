using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            if (TableController.Instance.TableSets[0].table.GetOrders()[0].OrderType == OrderType && PlayerOrderController.Instance.OrderList[1].OrderDataStructs.Count == 0)
            {
                OrderPanelController.Instance.PlayerOrderInventory(this);
                TutorialManager.Instance.SetTutorialInfo(7);
                return;
            }
            else
            {
                return;
            }
        }
        OrderPanelController.Instance.PlayerOrderInventory(this);
    }
    public void MinusButtonPressed()
    {
        OrderPanelController.Instance.RemoveOrderFromLPayer(this);

    }
}

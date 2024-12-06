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
        if (PlayerPrefsManager.Instance.LoadPlayerTutorialStep() == 5)
        {
            ControllerManager.Instance._orderPanelController.PlayerOrderInventory(this);
            TutorialManager.Instance.SetTutorialInfo(8);
            return;
        }
        ControllerManager.Instance._orderPanelController.PlayerOrderInventory(this);
    }
    public void MinusButtonPressed()
    {
        ControllerManager.Instance._orderPanelController.RemoveOrderFromLPayer(this);

    }
}

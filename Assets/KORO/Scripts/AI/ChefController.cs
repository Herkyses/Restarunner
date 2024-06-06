using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefController : MonoBehaviour,IInterectableObject
{
    [SerializeField] private List<OrderDataStruct> ChefOwnerStructData;
    [SerializeField] private Transform FoodParent;
    [SerializeField] private ChefOrderTable _chefOrderTable;
    [SerializeField] private int _chefOrderTableIndex;

    private void OnEnable()
    {
        GiveChefOrderPanelController.IsGivedToChef += SetOrders;
    }

    private void OnDisable()
    {
        GiveChefOrderPanelController.IsGivedToChef -= SetOrders;
    }

    public void SetOrders(List<OrderDataStruct> orderDataStruct)
    {
        ChefOwnerStructData = orderDataStruct;
        CreateFoods();
        orderDataStruct.Clear();
    }

    public void CreateFoods()
    {
        for (int i = 0; i < ChefOwnerStructData.Count; i++)
        {
            for (int j = 0; j < GameDataManager.Instance.Foods.Count; j++)
            {
                if (ChefOwnerStructData[i].OrderType == GameDataManager.Instance.Foods[j].OrderType)
                {
                    var food = Instantiate(GameDataManager.Instance.Foods[j]);
                    food.transform.position = _chefOrderTable.FoodTransformList[_chefOrderTableIndex].position;
                    _chefOrderTableIndex++;
                    if (_chefOrderTableIndex >= _chefOrderTable.FoodTransformList.Count)
                    {
                        _chefOrderTableIndex = 0;
                    }
                }
            }
        }
    }
    public void InterectableObjectRun()
    {
        
    }

    public void ShowOutline(bool active)
    {
        
    }

    public Outline GetOutlineComponent()
    {
        return null;
    }

    public string GetInterectableText()
    {
        Debug.Log("selamkee");
        return "GiveOrder";
    }
}

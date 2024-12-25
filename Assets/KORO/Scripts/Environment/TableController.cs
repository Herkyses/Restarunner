using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TableController : MonoBehaviour
{
    public static Action<Table> GivedOrderForAIWaiter;
    public List<TableSet> TableSets;
    public List<Transform> TableSetsTransforms;
    public Transform TableTransform;
    public TableSet TableSetPf;
    public ChefController ChefController;
    public int TableSetCapacity;
    [SerializeField] private MaterialData _materialData;
    [SerializeField] private TableSetData _tableSetData;
    [Inject] private DiContainer _container;



    public void Initialize()
    {
        SetTableNumbers();
    }

    public TableSetData GetTableSetData()
    {
        return _tableSetData;
    }
    public void SetTableNumbers()
    {
        GameObject[] tableObjects = GameObject.FindGameObjectsWithTag("TableSet");

        for (int i = 0; i < tableObjects.Length; i++)
        {
            var tableSet = tableObjects[i].GetComponent<TableSet>();
            TableSets.Add(tableSet);
            tableSet.table.TableNumber = i;
            tableSet.table.TableInitialize();
        }
    }

    public void UpdateTables()
    {
        for (int i = 0; i < TableSets.Count; i++)
        {
            TableSets[i].table.TableNumber = i;
            Debug.Log("tablenumberiii" + i);
            TableSets[i].table.TableInitialize();
        }
        PlayerOrderController.Instance.SetOrderList();  /// Oturan olmaması lazımmmmmm
    }

    public void EnableTableSetCollider(bool enabledValue)
    {
        for (int i = 0; i < TableSets.Count; i++)
        {
            TableSets[i].GetComponent<BoxCollider>().enabled = enabledValue;
        }
    }

    public void InjectTableObject(object table)
    {
        _container.Inject(table);
    }
    public Material GetSetableMaterial()
    {
        return _materialData.TrueMaterial;
    }
    public Material GetWrongMaterial()
    {
        return _materialData.WrongMaterial;
    }
}

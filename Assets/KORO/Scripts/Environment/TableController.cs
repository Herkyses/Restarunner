using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    public static Action<Table> GivedOrderForAIWaiter;
    public List<TableSet> TableSets;
    public List<Transform> TableSetsTransforms;
    public Transform TableTransform;
    public TableSet TableSetPf;
    public ChefController ChefController;
    public int TableSetCapacity;
    

    public void Initialize()
    {
        //SettableNumbers();
    }

    private void Start()
    {
        
    }

    /*public void SettableNumbers()
    {
        for (int i = 0; i < TableSetCapacity; i++)
        {
            var set = Instantiate(TableSetPf, TableTransform);
            TableSets.Add(set);
            set.transform.position = TableSetsTransforms[i].position;
            set.table.TableNumber = i + 1;
        }
    }*/

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
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    public List<TableSet> TableSets;
    public List<Transform> TableSetsTransforms;
    public Transform TableTransform;
    public TableSet TableSetPf;
    public int TableSetCapacity;

    private void Start()
    {
        SettableNumbers();
    }

    public void SettableNumbers()
    {
        for (int i = 0; i < TableSetCapacity; i++)
        {
            var set = Instantiate(TableSetPf, TableTransform);
            TableSets.Add(set);
            set.transform.position = TableSetsTransforms[i].position;
            set.table.TableNumber = i + 1;
        }
    }

    public void CreateTableSet()
    {
    }
}

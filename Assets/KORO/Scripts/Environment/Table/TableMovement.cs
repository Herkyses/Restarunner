using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableMovement : MonoBehaviour
{
    [SerializeField] private TableSetData tableSetData;
    [SerializeField] private Material _material;
    private TableController _tableController;
    private Material _currentMaterial;
    private GameSceneCanvas _gameSceneCanvas;
    
    public bool IsTableMove ;
    public bool IsTableSetTransform ;
    public bool IsTableInitiateForMove ;
    
    public List<Chair> ChairList;

    public TableSet TableSet;


    // Start is called before the first frame update
    public void Initiliaze(TableController tableController)
    {
        _tableController = tableController;
        _gameSceneCanvas = GameSceneCanvas.Instance;
        
    }

   
    public void InitiateTableMovement()
    {
        IsTableInitiateForMove = true;
        _gameSceneCanvas = _gameSceneCanvas ?? GameSceneCanvas.Instance;
        if (tableSetData == null)
        {
            tableSetData = _tableController.GetTableSetData();
            TableSet.table.InitializeTable();
        }
        _gameSceneCanvas.MoveObjectInfo(tableSetData.textsForTable, tableSetData.textsButtonsForTable, Enums.PlayerStateType.MoveTable,gameObject);
        TableSet.GetComponent<BoxCollider>().enabled = false;
        _tableController.EnableTableSetCollider(true);
        GetComponent<BoxCollider>().isTrigger = true;
    }
    public void SetChairMaterial(Material material)
    {
        for (int i = 0; i < ChairList.Count; i++)
        {
            ChairList[i].GetComponent<Renderer>().sharedMaterial = material;
        }
    }

    public void FinalizeTableMovement()
    {
        IsTableMove = false;
        IsTableSetTransform = false;
        IsTableInitiateForMove = false;
        _tableController.EnableTableSetCollider(false);
        GetComponent<BoxCollider>().isTrigger = false;
        GetComponent<Renderer>().sharedMaterial = _material;
        SetChairMaterial(_material);
        GameManager.Instance.HandleTablePlacementCompletion();
        TableSet.table.FinalizeTablePlacement();

    }
}

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
    private Renderer _renderer;
    
    public bool IsTableMove ;
    public bool IsTableSetTransform ;
    public bool IsTableInitiateForMove ;
    
    public List<Chair> ChairList;

    public TableSet TableSet;


    // Start is called before the first frame update
    public void Initiliaze(TableController tableController)
    {
        _tableController = tableController;
        _renderer = GetComponent<Renderer>();
        _gameSceneCanvas = GameSceneCanvas.Instance;
        
    }

    void Update()
    {
        if (IsTableMove)
        {
            SetTablePosition();
            if (!IsTableInitiateForMove && IsTableMove)
            {
                InitiateTableMovement();
            }
        }

        
    }
    private void InitiateTableMovement()
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
    public void SetTablePosition()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out var hit))
        {
            TableSet.transform.position = new Vector3(hit.point.x, 0.14f, hit.point.z);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            var tableRotat = TableSet.transform.rotation;
            var tableRotatTemp = Quaternion.Euler(new Vector3(tableRotat.eulerAngles.x,tableRotat.eulerAngles.y+90f,tableRotat.eulerAngles.z));

            TableSet.transform.rotation = tableRotatTemp;
        }
        PlaceTable();
    }
    public void PlaceTable()
    {
        // Mouse tıklamasını kontrol edin
        TableSet.CheckGround();

        if (Input.GetMouseButtonDown(0))
        {
            if (IsTableSetTransform)
            {
                FinalizeTableMovement();
            }
            return;
        }

        Material newMaterial = IsTableSetTransform
            ? _tableController.GetSetableMaterial()
            : _tableController.GetWrongMaterial();

        if (_currentMaterial != newMaterial)
        {
            _renderer.sharedMaterial = newMaterial;
            _currentMaterial = newMaterial;
            SetChairMaterial(newMaterial);
        }
    }
    public void SetChairMaterial(Material material)
    {
        for (int i = 0; i < ChairList.Count; i++)
        {
            ChairList[i].GetComponent<Renderer>().sharedMaterial = material;
        }
    }

    private void FinalizeTableMovement()
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectMoveController : MonoBehaviour
{
    public Transform MoveObjectTransform;
    public IMovable MoveableObject;

    public bool IsObjectPlaceable;
    public bool IsCheckAround;
    public int groundLayer;
    public int tableIndexLayer;
    
    private Material _currentMaterial;
    private Vector3 velocityVector = Vector3.zero;

    
    private Renderer[] _renderers;
    [SerializeField] private LayerMask _layerMask;

    // Start is called before the first frame update
    void Start()
    {
        groundLayer = LayerMask.NameToLayer("Ground");
        tableIndexLayer = LayerMask.NameToLayer("Decoration");
        
    }

    public void InitiliazeMoveableObject(Transform moveObjectTransform,IMovable iMovable)
    {
        
        MoveObjectTransform = moveObjectTransform;
        MoveableObject = iMovable;
        _renderers = iMovable.GetRenderers();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsCheckAround)
        {
            return;
        }
        CheckGround();
        CheckMaterial();
        SetMoveableObjectPosition();
        if (Input.GetMouseButtonDown(0))
        {
            if (IsObjectPlaceable)
            {
                MoveableObject.PlacedObject();
                IsCheckAround = false;
            }
        }
    }
    public void CheckMaterial()
    {
        // Mouse tıklamasını kontrol edin
        
        Material newMaterial = IsObjectPlaceable
            ? ControllerManager.Instance.Tablecontroller.GetSetableMaterial()
            : ControllerManager.Instance.Tablecontroller.GetWrongMaterial();

        if (_currentMaterial != newMaterial)
        {
            //_renderer.sharedMaterial = newMaterial;
            for (int i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].sharedMaterial = newMaterial;
            }
        }
    }
    public void CheckGround()
    {
        Collider[] colliders = Physics.OverlapSphere(MoveObjectTransform.position, 0.8f, _layerMask);

        bool checkControl = false;

        foreach (Collider collider in colliders)
        {
            Debug.Log("checkground");
            if (collider.gameObject == MoveObjectTransform.gameObject)
            {
                Debug.Log("checkgroundnext");
                continue;
            } 

            int layer = collider.gameObject.layer;

            if (layer != groundLayer && layer == tableIndexLayer)
            {
                IsObjectPlaceable = false;
                Debug.Log("checkgroundfalse");

                return;
            }

            if (layer == groundLayer && layer != tableIndexLayer)
            {
                checkControl = true;
                Debug.Log("checkgroundtrue");

            }
        }

        // Kontrol sonucunu belirle
        if (IsObjectPlaceable != checkControl)
        {
            IsObjectPlaceable = checkControl;
        }
    }
    public void SetMoveableObjectPosition()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out var hit))
        {
            Vector3 targetPosition = new Vector3(hit.point.x, 0.14f, hit.point.z);
            MoveObjectTransform.position = Vector3.SmoothDamp(MoveObjectTransform.position, targetPosition, ref velocityVector, 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            var tableRotat = MoveObjectTransform.rotation;
            var tableRotatTemp = Quaternion.Euler(new Vector3(tableRotat.eulerAngles.x,tableRotat.eulerAngles.y+90f,tableRotat.eulerAngles.z));

            MoveObjectTransform.rotation = tableRotatTemp;
        }
        //PlaceTable();
    }
}

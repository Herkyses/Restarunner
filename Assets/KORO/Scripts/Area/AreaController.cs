using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour,IAreaInfo
{
    [SerializeField] private string _areaName;
    [SerializeField] private string _canvasText;
    [SerializeField] private Transform _transformForPlay;
    [SerializeField] private Enums.AreaStateType _areaStateType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowInfo()
    {
        var areaInfo = new AreaInfo
        {
            AreaName = _areaName,
            CanvasText = _canvasText,
            TransformForPlay = _transformForPlay,
            AreaStateType = _areaStateType
        };
        GameSceneCanvas.Instance.ShowAreaInfo(areaInfo.CanvasText);
    }

    public Transform GetPlayTransform()
    {
        return _transformForPlay;
    }
}

public struct AreaInfo
{
    public string AreaName;
    public string CanvasText;
    public Transform TransformForPlay;
    public Enums.AreaStateType AreaStateType;
}
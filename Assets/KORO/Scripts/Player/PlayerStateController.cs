using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    public AreaInfo AreaInfo;
    public IAreaInfo _IAreaInfo;


    public void SetAreaInfo(AreaInfo areaInfo)
    {
        AreaInfo = areaInfo;
        switch (areaInfo.AreaStateType)
        {
            case Enums.AreaStateType.Door:
                Debug.Log("dooort");
                break;
            case Enums.AreaStateType.PlayArea:
                Debug.Log("PlayArea");
                break;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

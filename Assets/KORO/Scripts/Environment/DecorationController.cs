using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationController : MonoBehaviour
{

    [SerializeField] private int _decorationCount;
    
    public static DecorationController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    
    
}

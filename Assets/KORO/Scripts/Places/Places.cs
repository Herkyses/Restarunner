using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Places : MonoBehaviour
{
    public Transform DoorTransform;
    public static Places Instance;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

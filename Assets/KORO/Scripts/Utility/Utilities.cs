using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    public static void DeleteTransformchilds(Transform relatedTransform)
    {
        if(!relatedTransform)
        {
            return;
        }
        for(int i=0; i<relatedTransform.childCount; i++)
        {
            Destroy(relatedTransform.GetChild(i).gameObject);
        }
    }
}

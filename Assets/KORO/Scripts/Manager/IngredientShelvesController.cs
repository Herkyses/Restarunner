using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientShelvesController : MonoBehaviour
{
    public List<SingleGredientShelves> ShelvesList;
    public static IngredientShelvesController Instance;
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

    public void Initiliaze()
    {
        for (int i = 0; i < ShelvesList.Count; i++)
        {
            ShelvesList[i].Initiliaze();
        }
    }
}

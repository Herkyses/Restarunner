using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbishManager : MonoBehaviour
{
    public static RubbishManager Instance;
    [SerializeField] private int _rubbishLevel;
    [SerializeField] private List<Transform> _rubbishLevelsParents;
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
    public void Initiliaze()
    {
        _rubbishLevel = PlayerPrefsManager.Instance.LoadPlaceRubbishLevel();
        ActivateRubbishes();
    }

    public void UpdateRubbishLevel()
    {
        PlayerPrefsManager.Instance.SavePlaceRubbishLevel(_rubbishLevel+1);
        _rubbishLevel ++;
    }

    public bool CheckRubbishLevel()
    {
        var rubbishList = _rubbishLevelsParents[_rubbishLevel].GetComponentsInChildren<Rubbish>();
        if (rubbishList.Length > 0)
        {
            for (int i = 0; i < rubbishList.Length; i++)
            {
                if (rubbishList[i].gameObject.activeSelf)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void ActivateRubbishes()
    {
        _rubbishLevelsParents[PlayerPrefsManager.Instance.LoadPlaceRubbishLevel()].gameObject.SetActive(true);
    } 
    // Update is called once per frame
    void Update()
    {
        
    }
}

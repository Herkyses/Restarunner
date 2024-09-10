using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbishManager : MonoBehaviour
{
    public static RubbishManager Instance;
    [SerializeField] private int _rubbishLevel;
    [SerializeField] private float _cleanRate;
    [SerializeField] private int _allRubbishCount;
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
        if (_rubbishLevel == 1)
        {
            //var tutorialStep = PlayerPrefsManager.Instance.LoadPlayerTutorialStep();
            PlayerPrefsManager.Instance.SavePlayerPlayerTutorialStep(1);
            TutorialPanelController.Instance.ActivateRemainingText(false);
            TutorialManager.Instance.Initiliaze();

        }
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

    public int GetRubbishCount()
    {
        var rubbishList = _rubbishLevelsParents[_rubbishLevel].GetComponentsInChildren<Rubbish>();
        return rubbishList.Length;
    }

    public void ActivateRubbishes()
    {
        _rubbishLevelsParents[PlayerPrefsManager.Instance.LoadPlaceRubbishLevel()].gameObject.SetActive(true);
        _allRubbishCount = _rubbishLevelsParents[PlayerPrefsManager.Instance.LoadPlaceRubbishLevel()].childCount;
        CheckRubbishRate();
    }

    public void CheckRubbishRate()
    {
        _allRubbishCount = 0;

        foreach (Transform child in _rubbishLevelsParents[PlayerPrefsManager.Instance.LoadPlaceRubbishLevel()])
        {
            if (child.gameObject.activeInHierarchy)
            {
                _allRubbishCount++;
            }
        }
        _cleanRate = (float)_allRubbishCount/_rubbishLevelsParents[PlayerPrefsManager.Instance.LoadPlaceRubbishLevel()].childCount;
        GameSceneCanvas.Instance.SetCleanRateText(_cleanRate);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

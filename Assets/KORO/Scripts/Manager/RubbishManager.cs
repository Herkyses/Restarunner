using System;
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
    [SerializeField] private List<Transform> rubbishChilds;

    public static Action CheckedRubbishes;
    public static Action CheckedRubbishesForTutorial;


    private void OnEnable()
    {
        CheckedRubbishes += CheckRubbishRate;
        CheckedRubbishesForTutorial += CheckRubbishesForTutorial;
    }

    private void OnDisable()
    {
        CheckedRubbishes -= CheckRubbishRate;
        CheckedRubbishesForTutorial -= CheckRubbishesForTutorial;

    }

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
        var rubbishList = GetComponentsInChildren<Rubbish>();
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
        return transform.childCount;
    }

    public void ActivateRubbishes()
    {
        var playerprefsManager = PlayerPrefsManager.Instance;
        if (playerprefsManager.LoadPlaceRubbishLevel() == playerprefsManager.LoadPlaceLevel())
        {
            _rubbishLevelsParents[playerprefsManager.LoadPlaceRubbishLevel()].gameObject.SetActive(true);
            //var rubbishChilds = _rubbishLevelsParents[PlayerPrefsManager.Instance.LoadPlaceRubbishLevel()].GetComponentsInChildren<Transform>();
        
        
            foreach (Transform child in _rubbishLevelsParents[playerprefsManager.LoadPlaceRubbishLevel()].GetComponentsInChildren<Transform>())
            {
                // Kendisi de dahil olabilir, ayıklamak için:
                if (child.gameObject != _rubbishLevelsParents[playerprefsManager.LoadPlaceRubbishLevel()].gameObject)
                {
                    rubbishChilds.Add(child);
                }
            }
        
            for (int i = 0; i < rubbishChilds.Count; i++)
            {
                var rubbish = PoolManager.Instance.GetFromPoolForRubbish();
                //rubbish.transform.position = rubbishChilds[i].position;
                rubbish.transform.position = new Vector3(rubbishChilds[i].position.x,0.32f,rubbishChilds[i].position.z);
                rubbish.transform.SetParent(transform);
            }
            _allRubbishCount = _rubbishLevelsParents[playerprefsManager.LoadPlaceRubbishLevel()].childCount;
        }

        
        CheckRubbishRate();
        
    }

    public void CheckRubbishRate()
    {
        /*_allRubbishCount = 0;

        foreach (Transform child in _rubbishLevelsParents[PlayerPrefsManager.Instance.LoadPlaceLevel()])
        {
            if (child.gameObject.activeInHierarchy)
            {
                _allRubbishCount++;
            }
        }*/
        //TODO: mekan levele göre bütün çöplük oranı hesaplanması
        _cleanRate = (float)GetRubbishCount()/(float)GetRubbishChildDenominator();
        Debug.Log("getrubco: " + GetRubbishCount() + "GetRubbishChildDenominator: " + GetRubbishChildDenominator());
        GameSceneCanvas.Instance.SetCleanRateText(_cleanRate);
    }

    public int GetRubbishChildDenominator()
    {
        var count = 0;
        for (int i = 0; i <= PlayerPrefsManager.Instance.LoadPlaceLevel(); i++)
        {
            count +=_rubbishLevelsParents[i].childCount;
        }

        return count;
    }

    public void CheckRubbishesForTutorial()
    {
        if (PlayerPrefsManager.Instance.LoadPlaceRubbishLevel() == 0)
        {
            TutorialPanelController.Instance.SetRubbishCount();
        }
    }

    public void CreateRubbishFromAI()
    {
        var rubbish = PoolManager.Instance.GetFromPoolForRubbish();
        rubbish.transform.position = _rubbishLevelsParents[0].position;
    }

    public void ReturnRubbish(GameObject rubbishObject)
    {
        PoolManager.Instance.ReturnToPoolForRubbish(rubbishObject);
    }

    public float GetCleanRateValue()
    {
        return _cleanRate;
    }
}

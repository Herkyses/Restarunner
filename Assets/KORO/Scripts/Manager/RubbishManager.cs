using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RubbishManager : MonoBehaviour
{
    public static RubbishManager Instance;
    [SerializeField] private int _rubbishLevel;
    [SerializeField] private float _cleanRate;
    [SerializeField] private int _allRubbishCount;
    [SerializeField] private List<Transform> _rubbishLevelsParents;
    [SerializeField] private List<Transform> rubbishChilds;

    private const int TUTORIAL_CHECK_STEP = 4;

    public static Action CheckedRubbishes;
    public static Action CheckedRubbishesForTutorial;


    private void OnEnable()
    {
        CheckedRubbishes += CheckRubbishRate;
        CheckedRubbishesForTutorial += CheckRubbishesForTutorial;
        ShopManager.UpgradedRestaurant += CreateRubbishesForUpgraded;
    }

    private void OnDisable()
    {
        CheckedRubbishes -= CheckRubbishRate;
        CheckedRubbishesForTutorial -= CheckRubbishesForTutorial;
        ShopManager.UpgradedRestaurant -= CreateRubbishesForUpgraded;
    }

    public void CreateRubbishesForUpgraded()
    {
        ActivateRubbishes();
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
        CheckRubbishRate();
    }

    public void UpdateRubbishLevel()
    {
        _rubbishLevel = PlayerPrefsManager.Instance.LoadPlaceLevel();
        if (_rubbishLevel+1 == 1 && PlayerPrefsManager.Instance.LoadPlayerTutorialStep() < TUTORIAL_CHECK_STEP)
        {
            Debug.Log("rubbishlev" + _rubbishLevel);

            //var tutorialStep = PlayerPrefsManager.Instance.LoadPlayerTutorialStep();
            PlayerPrefsManager.Instance.SavePlayerPlayerTutorialStep(1);
            TutorialPanelController.Instance.ActivateRemainingText(false);
            TutorialManager.Instance.Initiliaze();

        }
        PlayerPrefsManager.Instance.SavePlaceRubbishLevel(_rubbishLevel+1);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            ActivateRubbishes();
        }
    }

    public bool CheckRubbishLevel()
    {
        var rubbishList = GetComponentsInChildren<Rubbish>();
        if (GetRubbishCount() > 0)
        {
            return false;

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
        var count = 0;
        for (int i = 0; i <= PlayerPrefsManager.Instance.LoadPlaceLevel(); i++)
        {
            for (int j = 0; j < _rubbishLevelsParents[i].GetComponentsInChildren<Rubbish>().Length; j++)
            {
                count += 1;

            }
        }
        var rubbishesFromGameobject = GetComponentsInChildren<Rubbish>();
        
        for (int j = 0; j < rubbishesFromGameobject.Length; j++)
        {
            count += 1;
                
        }

        return count;
        //return transform.childCount;
    }

    public void ActivateRubbishes()
    {
        var playerprefsManager = PlayerPrefsManager.Instance;
        
        for (int i = 0; i <= playerprefsManager.LoadPlaceLevel(); i++)
        {
            _rubbishLevelsParents[i].gameObject.SetActive(true);
        }


        for (int i = playerprefsManager.LoadPlaceRubbishLevel(); i <= playerprefsManager.LoadPlaceLevel(); i++)
        {
            var rubbishParents = _rubbishLevelsParents[i].GetComponentsInChildren<SingleRubbishParent>();
            for (int j = 0; j < rubbishParents.Length; j++)
            {
                if (rubbishParents[i].enabled)
                {
                    if (rubbishParents[j].transform.GetComponentsInChildren<Rubbish>().Length > 0)
                    {
                        continue;
                    }
                    var rubbish = PoolManager.Instance.GetFromPoolForRubbish();
                    //rubbish.transform.position = rubbishChilds[i].position;
                    rubbish.transform.position = new Vector3(rubbishParents[j].transform.position.x,0.32f,rubbishParents[j].transform.position.z);
                    rubbish.GetComponent<MeshFilter>().sharedMesh = GameDataManager.Instance.RubbishMeshes[Random.Range(0, GameDataManager.Instance.RubbishMeshes.Count)].sharedMesh;
                    rubbish.transform.SetParent(rubbishParents[j].transform);
                }
                
            }
        }

        

        
        /*if (playerprefsManager.LoadPlaceRubbishLevel() == playerprefsManager.LoadPlaceLevel())
        {
        _rubbishLevelsParents[playerprefsManager.LoadPlaceRubbishLevel()].gameObject.SetActive(true);
            //var rubbishChilds = _rubbishLevelsParents[PlayerPrefsManager.Instance.LoadPlaceRubbishLevel()].GetComponentsInChildren<Transform>();
        
            rubbishChilds.Clear();
            foreach (Transform child in _rubbishLevelsParents[playerprefsManager.LoadPlaceRubbishLevel()].GetComponentsInChildren<Transform>())
            {
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
                rubbish.GetComponent<MeshFilter>().sharedMesh = GameDataManager.Instance.RubbishMeshes[Random.Range(0, GameDataManager.Instance.RubbishMeshes.Count)].sharedMesh;
                rubbish.transform.SetParent(rubbishChilds[i]);
            }
            _allRubbishCount = _rubbishLevelsParents[playerprefsManager.LoadPlaceRubbishLevel()].childCount;
        //}//*/

        
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
        var playerprefsManager = PlayerPrefsManager.Instance;
        var transformList = new List<GameObject>();
        for (int i = 0; i <= playerprefsManager.LoadPlaceLevel(); i++)
        {
            var rubbishParents = _rubbishLevelsParents[i].GetComponentsInChildren<SingleRubbishParent>();
            for (int j = 0; j < rubbishParents.Length; j++)
            {
                if (rubbishParents[i].enabled)
                {
                    if (rubbishParents[j].transform.GetComponentsInChildren<Rubbish>().Length > 0)
                    {
                        continue;
                    }
                    
                    transformList.Add(rubbishParents[j].gameObject);
                }
                
            }
        }

        var randomIndex = Random.Range(0, transformList.Count);
        var rubbish = PoolManager.Instance.GetFromPoolForRubbish();
        var rubbishPos = transformList[randomIndex].transform.position;
        rubbish.transform.position = new Vector3(rubbishPos.x,0.32f,rubbishPos.z);
        rubbish.transform.SetParent(transform);
        CheckRubbishRate();
        /*var rubbish = PoolManager.Instance.GetFromPoolForRubbish();
        rubbish.transform.position = _rubbishLevelsParents[0].position;*/
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

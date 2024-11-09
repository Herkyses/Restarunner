using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RubbishManager : MonoBehaviour
{
    public static RubbishManager Instance;
    [SerializeField] private int _rubbishLevel;
    [SerializeField] private int _placeLevel;
    [SerializeField] private float _cleanRate;
    [SerializeField] private int _allRubbishCount;
    [SerializeField] private List<Transform> _rubbishLevelsParents;
    [SerializeField] private List<Transform> rubbishChilds;

    public GameObject rubbishPrefab;               // Çöp prefabı
    public List<Transform> spawnPoints;            // Belirlenen başlangıç transformları
    //private Queue<Transform> availablePoints;
    private List<Transform> availablePoints;// Boş olan transformları takip eden kuyruk
    private List<GameObject> spawnedRubbish;       // Oluşan çöpleri takip eden liste
    
    
    private const int TUTORIAL_CHECK_STEP = 4;

    public static Action CheckedRubbishes;
    public static Action CheckedRubbishesForTutorial;


    private void OnEnable()
    {
        CheckedRubbishes += CheckRubbishRate;
        CheckedRubbishesForTutorial += CheckRubbishesForTutorial;
        ShopManager.UpgradedRestaurant += Initiliaze;
    }

    private void OnDisable()
    {
        CheckedRubbishes -= CheckRubbishRate;
        CheckedRubbishesForTutorial -= CheckRubbishesForTutorial;
        ShopManager.UpgradedRestaurant -= Initiliaze;
    }

    public void CreateRubbishesForUpgraded()
    {
        //ActivateRubbishes();
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

    private void InitiliazeRubbishes()
    {
        _placeLevel = PlayerPrefsManager.Instance.LoadPlaceLevel();
        _rubbishLevel = PlayerPrefsManager.Instance.LoadPlaceRubbishLevel();
        var listdeneme = new List<Transform>();
        var counter = 0; 
        Debug.Log("level_rubbish " +_rubbishLevel);
        Debug.Log("level_place " +_placeLevel);
        for (int i = 0; i <= _placeLevel; i++)
        {
            var rubbishes = _rubbishLevelsParents[i].GetComponentsInChildren<SingleRubbishParent>();
            for (int j = 0; j < rubbishes.Length; j++)
            {
                spawnPoints.Add(rubbishes[j].transform);

            }
        }
        for (int j = 0; j < _rubbishLevel; j++)
        {
            var rubbishes = _rubbishLevelsParents[j].GetComponentsInChildren<SingleRubbishParent>();
            for (int i = 0; i < rubbishes.Length; i++)
            {
                counter++;
            }
        }
        availablePoints = new List<Transform>(spawnPoints);  // İlk pozisyonları kuyrukta başlat
        spawnedRubbish = new List<GameObject>();
        
        for (int i = counter; i < spawnPoints.Count; i++)
        {
            SpawnRubbish();
            
        }
        /*for (int i = 0; i < spawnPoints.Count; i++)
        {
            SpawnRubbish();
            
        }*/
    }

    public void CleanStarted()
    {
        if (CheckRubbishLevel())
        {
            Debug.Log("rubbishlevel: " + PlayerPrefsManager.Instance.LoadPlaceRubbishLevel());
            UpdateRubbishLevel();
            ActivateRubbishes();
        }
    }

    // Start is called before the first frame update
    public void Initiliaze()
    {
        _rubbishLevel = PlayerPrefsManager.Instance.LoadPlaceRubbishLevel();
        InitiliazeRubbishes();
        //ActivateRubbishes();
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
        if (Input.GetKeyDown(KeyCode.M))
        {
            CreateRubbishFromAI();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnRubbish();
        }
    }

    public bool CheckRubbishLevel()
    {
        var rubbishList = GetComponentsInChildren<Rubbish>();
        if (GetRubbishCount() > 0)
        {
            return false;
            
        }

        return true;
    }

    public int GetRubbishCount()
    {
        var count = 0;
        
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
        
        CheckRubbishRate();
        
    }

    public void CheckRubbishRate()
    {
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
    public void SpawnRubbish()
    {
        if (availablePoints.Count > 0)
        {
            /*Transform spawnPoint = availablePoints.Dequeue();
            GameObject rubbish = PoolManager.Instance.GetFromPoolForRubbish();
            rubbish.transform.position = spawnPoint.position;
            rubbish.transform.rotation = spawnPoint.rotation;
            rubbish.transform.SetParent(gameObject.transform);
            spawnedRubbish.Add(rubbish);  // Çöpler listesine ekle

            rubbish.GetComponent<Rubbish>().OnDestroyed += () => {
                availablePoints.Enqueue(spawnPoint); 
                spawnedRubbish.Remove(rubbish); 
                ReturnRubbish(rubbish);
            };*/
            
            
            
            int lastIndex = availablePoints.Count - 1;  // Son pozisyonun indexi
            Transform spawnPoint = availablePoints[lastIndex];  // Listenin sonundaki pozisyonu al
            availablePoints.RemoveAt(lastIndex);  // Son pozisyonu listeden çıkar
            
            GameObject rubbish = PoolManager.Instance.GetFromPoolForRubbish();  // Çöpü oluştur
            rubbish.transform.position = spawnPoint.position;
            rubbish.transform.rotation = spawnPoint.rotation;
            rubbish.transform.SetParent(gameObject.transform);
            rubbish.GetComponent<MeshFilter>().sharedMesh = GameDataManager.Instance.RubbishMeshes[Random.Range(0, GameDataManager.Instance.RubbishMeshes.Count)].sharedMesh;
            spawnedRubbish.Add(rubbish);  // Çöpler listesine ekle

            // Çöp temizlendiğinde pozisyonu tekrar kullanılabilir hale getir
            rubbish.GetComponent<Rubbish>().OnDestroyed += () => {
                availablePoints.Add(spawnPoint);  // Pozisyonu listeye geri ekle (sonda olacak)
                spawnedRubbish.Remove(rubbish);   // Çöpler listesinden kaldır
                ReturnRubbish(rubbish);

            };
        }
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

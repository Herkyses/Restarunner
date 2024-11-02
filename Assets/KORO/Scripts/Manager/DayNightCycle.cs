using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.PostProcessing;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance;
    
    [Header("Lighting Settings")]
    public Light directionalLight;
    public Gradient lightColor;  // Sabah, öğle, akşam için farklı renkler
    public AnimationCurve lightIntensity;  // Zamanla değişen ışık yoğunluğu
    
    [Header("Materials")]
    public Material lightMaterialNight;
    public Material lightMaterialDay;
    
    [Header("Post Processing")]
    public PostProcessVolume postProcessVolume;
    public PostProcessProfile postProcessDayProfile;
    public PostProcessProfile postProcessNightProfile;

    [Header("Time Settings")]
    public float elapsedTime = 0f;  
    public float transitionDuration;  
    public float transitionDelay;  
    public float twelveHoursInSeconds;  
    public int startHour = 9;   
    public int endHour = 21;    
    public float timeSpeed = 60.0f; 
    
    [Header("UI")]
    public TextMeshProUGUI timeText;

    [SerializeField] private Transform _lightParents;

    private float timeOfDay = 0f;
    private bool isNight = false;
    private bool startCycle = false;
    private bool canStartable = false;
    public bool IsNightBegun = false;

    private int currentHour;
    private int currentMinute;
    private float timeCounter = 0f;
    
    public List<GameObject> lightMaterialObjects;  // Zamanla değişen ışık yoğunluğu
    public Color ambientDayColor;
    public Color ambientNightColor;
    public float TwelvehoursSecond;  
    
    public float timeOfDayTEmp = 0f;

    
    private void OnEnable()
    {
        OpenCloseController.RestaurantOpened += InitiliazeCycle;
    }
    private void OnDisable()
    {
        OpenCloseController.RestaurantOpened -= InitiliazeCycle;

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

    private void Start()
    {
        twelveHoursInSeconds = 43200;
        IsNightBegun = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            InitiliazeCycle();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCycle();
        }
        
        if (!startCycle)
        {
            return;
        }
        
        UpdateTime();
        UpdateLightingCycle();
        
        
    }
    public void InitiliazeCycle()
    {
        if (!startCycle && !canStartable)
        {
            ResetTime();
            ApplyDayProfile();
            ToggleLights(false);
            UpdateMaterials(lightMaterialDay);
            startCycle = true;
            canStartable = true;
            IsNightBegun = false;
        }
        
    }
    
    private void StartCycle()
    {
        timeOfDay = 0f;
        ApplyDayProfile();
        Debug.Log("Day cycle started.");
    }
    
    private void ResetTime()
    {
        currentHour = startHour;
        currentMinute = 0;
        timeOfDay = 0f;
        isNight = false;
        postProcessVolume.profile = postProcessDayProfile;
    }
    
    private void UpdateTime()
    {
        timeCounter += Time.deltaTime * twelveHoursInSeconds / (transitionDuration);

        if (timeCounter >= 60f)
        {
            currentMinute++;
            //timeCounter = 0f;
            timeCounter -= 60f;

            if (currentMinute >= 60)
            {
                currentMinute = 0;
                currentHour++;
            }

            if (currentHour >= endHour)
            {
                currentHour = endHour;
                currentMinute = 0;
            }
        }

        UpdateTimeUI();
    }
    
    private void UpdateTimeUI()
    {
        if (timeText != null)
            timeText.text = string.Format("{0:00}:{1:00}", currentHour, currentMinute);
    }
    private void UpdateLightingCycle()
    {
        timeOfDay += Time.deltaTime;

        if (!isNight && timeOfDay >= transitionDelay)
        {
            StartCoroutine(SwitchToNight());
            isNight = true;
        }
    }
    
    private IEnumerator SwitchToNight()
    {
        yield return TransitionPostProcessingProfile(postProcessNightProfile);
        ToggleLights(true);
        UpdateMaterials(lightMaterialNight);
        startCycle = false;
        IsNightBegun = true;
    }
    private IEnumerator TransitionPostProcessingProfile(PostProcessProfile targetProfile)
    {
        yield return ChangePostProcessWeight(0.6f, 0.05f);
        postProcessVolume.profile = targetProfile;
        yield return ChangePostProcessWeight(0.05f, 0.6f);
    }
    private IEnumerator ChangePostProcessWeight(float startWeight, float endWeight)
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration / 2f)
        {
            elapsedTime += Time.deltaTime;
            postProcessVolume.weight = Mathf.Lerp(startWeight, endWeight, elapsedTime / (transitionDuration / 2f));
            yield return null;
        }
    }
    private void ToggleLights(bool state)
    {
        foreach (Light light in _lightParents.GetComponentsInChildren<Light>())
            light.enabled = state;
    }
    private void ApplyDayProfile()
    {
        postProcessVolume.profile = postProcessDayProfile;
    
        ToggleLights(false);

        UpdateMaterials(lightMaterialDay);
        canStartable = false;
        Debug.Log("Day profile applied.");
    }
    
    private void UpdateMaterials(Material newMaterial)
    {
        foreach (GameObject obj in lightMaterialObjects)
            obj.GetComponent<MeshRenderer>().material = newMaterial;
    }
    
}

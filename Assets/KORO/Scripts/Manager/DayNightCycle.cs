using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.PostProcessing;

public class DayNightCycle : MonoBehaviour
{
    public Light directionalLight;
    public PostProcessVolume postProcessVolume;
    public PostProcessVolume postProcessVolumeNight;
    public PostProcessProfile postProcessDayProfile;
    public PostProcessProfile postProcessNightProfile;
    public Gradient lightColor;  // Sabah, öğle, akşam için farklı renkler
    public AnimationCurve lightIntensity;  // Zamanla değişen ışık yoğunluğu
    public Material lightMaterialNight;  // Zamanla değişen ışık yoğunluğu
    public Material lightMaterialDay;  // Zamanla değişen ışık yoğunluğu
    public List<GameObject> lightMaterialObject;  // Zamanla değişen ışık yoğunluğu
    public Color ambientDayColor;
    public Color ambientNightColor;
    public float elapsedTime = 0f;  
    public float transitionDuration;  
    public float TransitionDurationDelay;  
    public float TwelvehoursSecond;  
    public bool StartCycle = false; 
    
    private float timeOfDay = 0f;
    public float timeOfDayTEmp = 0f;
    private bool isNight = false;
    [SerializeField] private Transform _lightParents;


    // Saat başlangıcı ve bitişi
    public int startHour = 9;   
    public int endHour = 21;    

    // Zaman ilerleme hızı
    public float timeSpeed = 60.0f; 
    private float timeCounter = 0f;

    // Şu anki saat ve dakika
    private int currentHour;
    private int currentMinute;

    // UI Text eklerseniz saati göstermek için kullanabilirsiniz
    public TextMeshProUGUI timeText;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            InitiliazeCycle();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCycleTrue();
        }
        
        if (!StartCycle)
        {
            return;
        }
        StartTime();
        
        
        timeOfDay += Time.deltaTime;
        // Gündüzden geceye geçiş
        if (!isNight && timeOfDay >= TransitionDurationDelay) // 60 saniye gündüz
        {
            StartCoroutine(WeightResetAndChangeProfile(postProcessNightProfile));
            isNight = true;  // Gece oldu
        }
        /*// Geceden gündüze geçiş
        else if (isNight && timeOfDay >= 60f) // 60 saniye gece
        {
            StartCoroutine(WeightResetAndChangeProfile(postProcessDayProfile));
            timeOfDay = 0f; // Yeni gün başlasın
            isNight = false;  // Gündüz oldu
        }*/
        
        
    }

    public void ResetDay()
    {
        timeOfDay = 0f; // Yeni gün başlasın
        isNight = false;  // Gündüz oldu
    }
    // İki profil arasında yumuşak geçiş yapar
    // Weight sıfırlayıp profili değiştirme, ardından tekrar 1'e çıkarma
    IEnumerator WeightResetAndChangeProfile(PostProcessProfile targetProfile)
    {
        float elapsedTime = 0f;

        // Fade-out (Mevcut profilin weight'ini 1'den 0'a düşür)
        while (elapsedTime < transitionDuration / 2f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (transitionDuration / 2f);
            postProcessVolume.weight = Mathf.Lerp(0.6f, 0.05f, t);
            yield return null;  // Bir sonraki frame'e kadar bekle
        }

        // Profil değiştirme
        postProcessVolume.profile = targetProfile;

        // Zamanı sıfırla ve fade-in başlat
        elapsedTime = 0f;

        // Fade-in (Yeni profilin weight'ini 0'dan 1'e çıkar)
        while (elapsedTime < transitionDuration / 2f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (transitionDuration / 2f);
            postProcessVolume.weight = Mathf.Lerp(0.05f, 0.6f, t);
            yield return null;  // Bir sonraki frame'e kadar bekle
        }
        Debug.Log("geceoldu");
        var lights = _lightParents.GetComponentsInChildren<Light>();
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].gameObject.GetComponent<Light>().enabled = true;
        }

        for (int i = 0; i < lightMaterialObject.Count; i++)
        {
            lightMaterialObject[i].gameObject.GetComponent<MeshRenderer>().material = lightMaterialNight;
        }

        StartCycle = false;
    }

    public void InitiliazeCycle()
    {
        // Başlangıç saatini ve dakikasını ayarla
        currentHour = startHour;
        currentMinute = 0;
        ResetDay();
        postProcessVolume.profile = postProcessDayProfile;
        StartCycle = true;
        Debug.Log("GunBasladi");
        var lights = _lightParents.GetComponentsInChildren<Light>();
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].gameObject.GetComponent<Light>().enabled = false;
        }
        for (int i = 0; i < lightMaterialObject.Count; i++)
        {
            lightMaterialObject[i].gameObject.GetComponent<MeshRenderer>().material = lightMaterialDay;
        }
    }

    public void StartCycleTrue()
    {
        timeOfDay = 0f;
        postProcessVolume.profile = postProcessDayProfile;
        Debug.Log("Sabaholdu");
    }

    public void StartTime()
    {
        // Zamanı güncelle
        timeCounter += Time.deltaTime * timeSpeed*(((TwelvehoursSecond/(transitionDuration+TransitionDurationDelay))/60f)+1f);

        // Dakikaları hesapla (Her 60 birimde bir dakika artar)
        if (timeCounter >= 60f)
        {
            currentMinute++;
            timeCounter = 0f;

            // Eğer dakika 60'a ulaştıysa, bir saat ekle ve dakikayı sıfırla
            if (currentMinute >= 60)
            {
                currentMinute = 0;
                currentHour++;
            }

            // Saat akşam 9'u geçtiğinde başa döner (sabah 9)
            if (currentHour == endHour)
            {
                currentHour = endHour;
                currentMinute = 0;
            }
        }

        // UI Text kullanıyorsanız, saati ve dakikayı güncelle
        if (timeText != null)
        {
            // Dakikanın çift haneli olmasını sağlamak için string formatı kullanıyoruz
            timeText.text = string.Format("{0:00}:{1:00}", currentHour, currentMinute);
        }
    }
}

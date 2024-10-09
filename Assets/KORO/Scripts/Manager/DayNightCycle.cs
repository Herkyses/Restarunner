using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Color ambientDayColor;
    public Color ambientNightColor;
    public float dayDuration = 10f;  // Bir gün kaç saniye sürecek
    public float elapsedTime = 0f;  // Bir gün kaç saniye sürecek
    public float transitionDuration = 15f;  // Bir gün kaç saniye sürecek
    public bool StartCycle = false; 
    
    private float timeOfDay = 0f;
    private bool isNight = false;   
    
    private float currentTime = 0f;
    
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
        /*currentTime += Time.deltaTime;
        float timePercent = (currentTime % dayDuration) / dayDuration;

        // Directional light rengi ve yoğunluğu
        directionalLight.color = lightColor.Evaluate(timePercent);
        directionalLight.intensity = lightIntensity.Evaluate(timePercent);

        // Ortam ışığını değiştirme
        /*if (timePercent < 0.5f)
        {
            RenderSettings.ambientLight = Color.Lerp(ambientNightColor, ambientDayColor, timePercent * 2f);
        }
        else
        {
            RenderSettings.ambientLight = Color.Lerp(ambientDayColor, ambientNightColor, (timePercent - 0.5f) * 2f);
        }#1#
        if (postProcessVolume.profile.TryGetSettings(out ColorGrading colorGrading))
        {
            colorGrading.temperature.value = Mathf.Lerp(-10f, 10f, timePercent);
            colorGrading.saturation.value = Mathf.Lerp(-30f, 20f, timePercent);
        }
        if (timePercent > 0.5f && timePercent < 0.9f)
        {
            //RenderSettings.ambientLight = Color.Lerp(ambientDayColor, ambientNightColor, (timePercent - 0.5f) * 2f);
            RenderSettings.ambientLight = Color.Lerp(ambientDayColor, ambientNightColor, (timePercent - 0.5f) * 2f);
        }
        else
        {
            StartCycle = false;
        }

        // Post Process ayarlarını değiştir*/
        // Zamanı ilerlet
        // Zamanı ilerlet
        timeOfDay += Time.deltaTime;

        // Gündüzden geceye geçiş
        if (!isNight && timeOfDay >= 30f) // 60 saniye gündüz
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
    }

    public void InitiliazeCycle()
    {
        ResetDay();
        postProcessVolume.profile = postProcessDayProfile;
        currentTime = dayDuration / 2;
        StartCycle = true;
        
    }

    public void StartCycleTrue()
    {
        timeOfDay = 0f;
        postProcessVolume.profile = postProcessDayProfile;
        currentTime = dayDuration / 2;
    }
}

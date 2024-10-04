using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DayNightCycle : MonoBehaviour
{
    public Light directionalLight;
    public PostProcessVolume postProcessVolume;
    public Gradient lightColor;  // Sabah, öğle, akşam için farklı renkler
    public AnimationCurve lightIntensity;  // Zamanla değişen ışık yoğunluğu
    public Color ambientDayColor;
    public Color ambientNightColor;
    public float dayDuration = 60f;  // Bir gün kaç saniye sürecek
    public bool StartCycle = false; 
    
    private float currentTime = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            InitiliazeCycle();
        }
        if (!StartCycle)
        {
            return;
        }
        currentTime += Time.deltaTime;
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
        }*/
        if (postProcessVolume.profile.TryGetSettings(out ColorGrading colorGrading))
        {
            colorGrading.temperature.value = Mathf.Lerp(-10f, 10f, timePercent);
            colorGrading.saturation.value = Mathf.Lerp(-30f, 20f, timePercent);
        }
        if (timePercent > 0.5f)
        {
            RenderSettings.ambientLight = Color.Lerp(ambientDayColor, ambientNightColor, (timePercent - 0.5f) * 2f);
        }
        else
        {
            StartCycle = false;
        }

        // Post Process ayarlarını değiştir
        
    }

    public void InitiliazeCycle()
    {
        StartCycle = true;
        currentTime = dayDuration / 2;
    }
}

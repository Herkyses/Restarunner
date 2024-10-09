using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public float timeOfDayTEmp = 0f;
    private bool isNight = false;   
    
    private float currentTime = 0f;
    
    // Saat başlangıcı ve bitişi
    public int startHour = 9;   // Sabah 9
    public int endHour = 21;    // Akşam 9 (24 saat diliminde 21)

    // Zaman ilerleme hızı
    public float timeSpeed = 60.0f; // 60 saniyede 1 oyun saati
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
        Debug.Log("geceoldu");
        StartCycle = false;
    }

    public void InitiliazeCycle()
    {
        // Başlangıç saatini ve dakikasını ayarla
        currentHour = startHour;
        currentMinute = 0;
        ResetDay();
        postProcessVolume.profile = postProcessDayProfile;
        currentTime = dayDuration / 2;
        StartCycle = true;
        Debug.Log("GunBasladi");
        
    }

    public void StartCycleTrue()
    {
        timeOfDay = 0f;
        postProcessVolume.profile = postProcessDayProfile;
        currentTime = dayDuration / 2;
        Debug.Log("Sabaholdu");
    }

    public void StartTime()
    {
        // Zamanı güncelle
        timeCounter += Time.deltaTime * timeSpeed*17;

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
            timeText.text = string.Format("Time: {0:00}:{1:00}", currentHour, currentMinute);
        }
    }
}

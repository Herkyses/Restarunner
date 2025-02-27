using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioMixerGroup  _sfxMixerGroup;

    
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
        // AudioSource listesi oluşturuluyor ve statik AudioSource nesneleri ekleniyor
        DontDestroyOnLoad(this.gameObject);

        audioSources = new List<AudioSource>();
        for (int i = 0; i < maxAudioSources; i++)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.outputAudioMixerGroup = _sfxMixerGroup;
            audioSources.Add(newSource);
        }

    }
    [SerializeField] private List<AudioClip> audioClips; // Ses kliplerinin listesi
    private List<AudioSource> audioSources; // AudioSource nesneleri listesi
    [SerializeField] private int maxAudioSources = 10; // Maksimum AudioSource sayısı
    
    // Boş bir AudioSource bulup sesi çalma işlemi
    public void PlaySound(int clipIndex, float volume = 1f)
    {
        if (volume == 0)
        {
            return;
        }
        if (clipIndex < 0 || clipIndex >= audioClips.Count)
        {
            Debug.LogWarning("Geçersiz ses indeksi!");
            return;
        }

        AudioSource availableSource = GetAvailableAudioSource();
        if (availableSource != null)
        {
            availableSource.clip = audioClips[clipIndex];
            availableSource.volume = volume;
            availableSource.loop = true;
            availableSource.Play();

            // Ses tamamlandığında AudioSource'u temizlemek için coroutine başlatılıyor
            StartCoroutine(ClearClipAfterPlay(availableSource));
        }
        else
        {
            Debug.LogWarning("Mevcut çalınacak boş AudioSource bulunamadı!");
        }
    }

    // Boş olan bir AudioSource bulma
    private AudioSource GetAvailableAudioSource()
    {
        foreach (var source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return null; // Hepsi doluysa null döndür
    }

    // Ses tamamlandığında clip'i temizlemek için coroutine
    private IEnumerator ClearClipAfterPlay(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);
        source.clip = null; // Ses tamamlandığında clip temizlenir
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            PlaySound(0,1f);
        }
    }
}

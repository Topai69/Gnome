using UnityEngine;
using System.Collections;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance;

    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioSource musicSource;
    
    [SerializeField] private float maxVolume = 1f;
    [SerializeField] private float lowVolume = 0.3f;
    [SerializeField] private float fadeDuration = 1.0f;
    
    private float currentVolumeMultiplier = 1f;
    private Coroutine fadeCoroutine;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();
            
        SetupMusicSource();
    }
    
    private void SetupMusicSource()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = PlayerPrefs.GetFloat("musicVolume", 1f) * maxVolume;
        musicSource.Play();
    }
    
    public void UpdateVolume(float sliderValue)
    {
        float targetVolume = sliderValue * maxVolume * currentVolumeMultiplier;
        musicSource.volume = targetVolume;
    }
    
    public void FadeToLow()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
            
        fadeCoroutine = StartCoroutine(FadeVolume(lowVolume, fadeDuration));
    }
    
    public void FadeToNormal()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
            
        fadeCoroutine = StartCoroutine(FadeVolume(maxVolume, fadeDuration));
    }
    
    private IEnumerator FadeVolume(float targetMultiplier, float duration)
    {
        float startMultiplier = currentVolumeMultiplier;
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            currentVolumeMultiplier = Mathf.Lerp(startMultiplier, targetMultiplier, elapsedTime / duration);
            musicSource.volume = PlayerPrefs.GetFloat("musicVolume", 1f) * currentVolumeMultiplier;
            
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        
        currentVolumeMultiplier = targetMultiplier;
        musicSource.volume = PlayerPrefs.GetFloat("musicVolume", 1f) * currentVolumeMultiplier;
    }
}
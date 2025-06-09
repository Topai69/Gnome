using UnityEngine;

public class WaterDripping : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] public AudioClip voiceLine;
    [SerializeField] private float volumeMultiplier = 0.5f;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        UpdateVolume();
        if (BackgroundMusicManager.Instance != null)
        {
            BackgroundMusicManager.Instance.OnVolumeChanged += UpdateVolume;
        }
    }

    void OnDestroy()
    {
        if (BackgroundMusicManager.Instance != null)
        {
            BackgroundMusicManager.Instance.OnVolumeChanged -= UpdateVolume;
        }
    }

    public void UpdateVolume()
    {
        if (audioSource != null)
        {
            float globalVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
            audioSource.volume = globalVolume * volumeMultiplier;

            if (BackgroundMusicManager.Instance != null)
            {
                audioSource.mute = BackgroundMusicManager.Instance.IsMuted();
            }
        }
    }

    // Update is called once per frame
    public void Stop()
    {
        if (audioSource != null)
            audioSource.Stop();
    }
}

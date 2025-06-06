using UnityEngine;
using UnityEngine.UI;
public class AudioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Slider audioSlider;
    public GameObject gameplayUI;
    public bool settingsUI;
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && gameplayUI.activeInHierarchy)
        {
            gameplayUI.SetActive(false);
            settingsUI = true;
        }
        if (settingsUI == true && Input.GetKeyDown(KeyCode.Escape))
        {
            gameplayUI.SetActive(true);
            settingsUI = false;
        }
    }
    public void ChangeVolume()
    {
        AudioListener.volume = audioSlider.value;
        Save();
    }

    private void Load()
    {
        audioSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", audioSlider.value);
    }
}

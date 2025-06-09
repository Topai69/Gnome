using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button volumeUpButton;
    [SerializeField] private Button volumeDownButton;
    [SerializeField] private Button musicToggleButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject gameplayUIElements;
    [SerializeField] private TextMeshProUGUI volumePercentageText;
    [SerializeField] private TextMeshProUGUI musicToggleText;
    [SerializeField] private TextMeshProUGUI timeRemainingText;
    [SerializeField] private Image batteryFillImage;
    
    [Header("Volume Settings")]
    [SerializeField] private float volumeIncrement = 0.1f;
    
    private bool isMusicOn = true;
    public bool isPaused = false;
    private HintSystem hintSystem;
    private bool usingController = false;
    private GameCountdownTimer gameTimer;
    private TaskManager taskManager;
    private PlayerMovement playerMovement;
    
    void Start()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
            
        hintSystem = FindObjectOfType<HintSystem>();
        gameTimer = FindObjectOfType<GameCountdownTimer>();
        taskManager = FindObjectOfType<TaskManager>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        
        SetupButtonNavigation();

        if (volumePercentageText != null)
            UpdateVolumeText();
    }
    
    void SetupButtonNavigation()
    {
        if (quitButton == null || resumeButton == null || volumeUpButton == null || 
            musicToggleButton == null || volumeDownButton == null)
            return;
        
        //quit
        Navigation quitNav = new Navigation();
        quitNav.mode = Navigation.Mode.Explicit;
        quitNav.selectOnDown = resumeButton;
        quitNav.selectOnRight = volumeUpButton;
        quitButton.navigation = quitNav;
        
        //resume
        Navigation resumeNav = new Navigation();
        resumeNav.mode = Navigation.Mode.Explicit;
        resumeNav.selectOnUp = quitButton;
        resumeNav.selectOnRight = volumeDownButton;
        resumeButton.navigation = resumeNav;
        
        //vol up
        Navigation volumeUpNav = new Navigation();
        volumeUpNav.mode = Navigation.Mode.Explicit;
        volumeUpNav.selectOnDown = musicToggleButton;
        volumeUpNav.selectOnLeft = quitButton;
        volumeUpButton.navigation = volumeUpNav;
        
        //pause music
        Navigation musicToggleNav = new Navigation();
        musicToggleNav.mode = Navigation.Mode.Explicit;
        musicToggleNav.selectOnUp = volumeUpButton;
        musicToggleNav.selectOnDown = volumeDownButton;
        musicToggleNav.selectOnLeft = quitButton;
        musicToggleButton.navigation = musicToggleNav;
        
        //volume down
        Navigation volumeDownNav = new Navigation();
        volumeDownNav.mode = Navigation.Mode.Explicit;
        volumeDownNav.selectOnUp = musicToggleButton;
        volumeDownNav.selectOnLeft = resumeButton;
        volumeDownButton.navigation = volumeDownNav;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            TogglePause();
        }
        
        if (isPaused && timeRemainingText != null && gameTimer != null)
        {
            UpdateTimeDisplay();
        }
        
        if (isPaused)
        {
            UpdateBatteryFill();
            
            bool mouseMovement = Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f;
            bool mouseClick = Input.GetMouseButtonDown(0);
            
            bool controllerInput = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f || 
                                  Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f ||
                                  Input.GetButtonDown("Submit") || 
                                  Input.GetButtonDown("Cancel");
            
            if (mouseMovement || mouseClick)
            {
                if (usingController)
                {
                    usingController = false;
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
            else if (controllerInput)
            {
                usingController = true;
                if (EventSystem.current.currentSelectedGameObject == null && resumeButton != null)
                {
                    EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
                }
            }
        }
    }
    
    private void UpdateTimeDisplay()
    {
        if (timeRemainingText != null && gameTimer != null)
        {
            int minutes = Mathf.FloorToInt(gameTimer.timeLeftInSeconds / 60);
            int seconds = Mathf.FloorToInt(gameTimer.timeLeftInSeconds % 60);
            timeRemainingText.text = $"{minutes:00}:{seconds:00}";
        }
    }
    
    private void UpdateBatteryFill()
    {
        if (batteryFillImage != null && taskManager != null)
        {
            float progress = taskManager.GetCurrentProgress();
            batteryFillImage.fillAmount = progress;
        }
    }
    
    public void TogglePause()
    {
        isPaused = !isPaused;
        
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(isPaused);
        
        if (gameplayUIElements != null)
            gameplayUIElements.SetActive(!isPaused);
            
        if (isPaused)
        {
            BlockPlayerMovement();
            
            UpdateTimeDisplay();
            UpdateBatteryFill();
            
            if (resumeButton != null)
                StartCoroutine(SelectButtonNextFrame(resumeButton.gameObject));
                
            if (BackgroundMusicManager.Instance != null)
                BackgroundMusicManager.Instance.FadeToLow();
        }
        else
        {
            StartCoroutine(SafelyUnblockPlayerMovement());
            
            if (BackgroundMusicManager.Instance != null)
                BackgroundMusicManager.Instance.FadeToNormal();
        }
        
        Time.timeScale = isPaused ? 0f : 1f;
        
        if (hintSystem != null)
        {
            if (isPaused)
                hintSystem.OnGamePaused();
            else
                hintSystem.OnGameResumed();
        }
    }
    
    private void BlockPlayerMovement()
    {
        if (playerMovement != null)
        {
            playerMovement.blockAInput = true;
            playerMovement.blockJump = true;
            playerMovement.enabled = false;
        }
    }
    
    private System.Collections.IEnumerator SafelyUnblockPlayerMovement()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return null;
        }
        
        if (playerMovement != null)
        {
            playerMovement.blockAInput = false;
            playerMovement.blockJump = false;
            playerMovement.enabled = true;
        }
    }
    
    private System.Collections.IEnumerator SelectButtonNextFrame(GameObject button)
    {
        yield return null;
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }
    
    public void ResumeGame()
    {
        TogglePause();
    }
    
    public void IncreaseVolume()
    {
        float currentVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        float newVolume = Mathf.Clamp01(currentVolume + volumeIncrement);
        
        AudioListener.volume = newVolume;
        PlayerPrefs.SetFloat("musicVolume", newVolume);
        
        if (BackgroundMusicManager.Instance != null)
            BackgroundMusicManager.Instance.UpdateVolume(newVolume);
            
        UpdateVolumeText();
    }
    
    public void DecreaseVolume()
    {
        float currentVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        float newVolume = Mathf.Clamp01(currentVolume - volumeIncrement);
        
        AudioListener.volume = newVolume;
        PlayerPrefs.SetFloat("musicVolume", newVolume);
        
        if (BackgroundMusicManager.Instance != null)
            BackgroundMusicManager.Instance.UpdateVolume(newVolume);
            
        UpdateVolumeText();
    }
    
    private void UpdateVolumeText()
    {
        if (volumePercentageText != null)
        {
            int percentage = Mathf.RoundToInt(PlayerPrefs.GetFloat("musicVolume", 1f) * 100);
            volumePercentageText.text = percentage + "%";
        }
    }
    
    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        
        if (BackgroundMusicManager.Instance != null)
        {
            if (isMusicOn)
            {
                BackgroundMusicManager.Instance.ResumeMusic();
                if (musicToggleText != null)
                    musicToggleText.text = "Music: ON";
            }
            else
            {
                BackgroundMusicManager.Instance.PauseMusic();
                if (musicToggleText != null)
                    musicToggleText.text = "Music: OFF";
            }
        }
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
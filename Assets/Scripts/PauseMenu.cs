using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused = false;

    [Header("Menu Panels")]
    public GameObject pauseMenuPanel;
    public GameObject gameplayUI;

    public TextMeshProUGUI timeLeft;
    public RotateOverTime clock;

    private HintSystem hintSystem;

    void Start()
    {
        pauseMenuPanel.SetActive(false);
        hintSystem = FindObjectOfType<HintSystem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause(); 
        }
        timeLeft.text = "Time Left:" + (clock.gameDuration - clock.elapsedTime).ToString("F0");
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);
        gameplayUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
        gameplayUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }

        if (isPaused && hintSystem != null)
        {
            hintSystem.OnGamePaused();
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    
}
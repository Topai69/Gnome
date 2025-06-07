using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private Button resumeButton; 

    public bool isPaused = false;

    [Header("Menu Panels")]
    public GameObject gameplayUI;

    public TextMeshProUGUI timeLeft;
    public RotateOverTime clock;

    private HintSystem hintSystem;

    private bool usingController = false;

    void Start()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        hintSystem = FindObjectOfType<HintSystem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            TogglePause();
        }

        if (isPaused)
        {
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

        if (timeLeft != null && clock != null)
        {
            timeLeft.text = "Time Left:" + (clock.gameDuration - clock.elapsedTime).ToString("F0");
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(isPaused);
            if (isPaused && resumeButton != null)
            {
                StartCoroutine(SelectButtonNextFrame(resumeButton.gameObject));
            }
        }

        // Blocca/sblocca il tempo di gioco
        Time.timeScale = isPaused ? 0f : 1f;

        // Gestione dell'HintSystem
        if (hintSystem != null)
        {
            if (isPaused)
                hintSystem.OnGamePaused();
            else
                hintSystem.OnGameResumed();
        }
    }

    private System.Collections.IEnumerator SelectButtonNextFrame(GameObject button)
    {
        yield return null; 

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);

        PauseButtonHighlight highlight = button.GetComponent<PauseButtonHighlight>();
        if (highlight != null)
        {
            highlight.OnSelect(new BaseEventData(EventSystem.current));
        }
    }

    public void ResumeGame()
    {
        TogglePause();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        // SceneManager.LoadScene("MainMenu");
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
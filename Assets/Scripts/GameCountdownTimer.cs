using UnityEngine;
using TMPro;

public class GameCountdownTimer : MonoBehaviour
{
    public float timeLeftInSeconds = 600f; 
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private bool useUnscaledTime = true;
    
    private bool isRunning = true;
    private float lastUpdateTime;

    void Start()
    {
        lastUpdateTime = useUnscaledTime ? Time.unscaledTime : Time.time;
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (isRunning)
        {
            float currentTime = useUnscaledTime ? Time.unscaledTime : Time.time;
            float deltaTime = currentTime - lastUpdateTime;
            lastUpdateTime = currentTime;

            if (timeLeftInSeconds > 0)
            {
                timeLeftInSeconds -= deltaTime;
                if (timeLeftInSeconds < 0)
                    timeLeftInSeconds = 0;
            }
            
            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeLeftInSeconds / 60);
            int seconds = Mathf.FloorToInt(timeLeftInSeconds % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}

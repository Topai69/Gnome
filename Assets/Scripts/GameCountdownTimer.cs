using TMPro;
using UnityEngine;

public class GameCountdownTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeLeftInSeconds = 600f; 

    [Header("UI Reference")]
    public TextMeshProUGUI timeText;

    private bool isRunning = true;

    void Update()
    {
        if (!isRunning || timeText == null) return;

        if (timeLeftInSeconds > 0f)
        {
            timeLeftInSeconds -= Time.unscaledDeltaTime;
            timeLeftInSeconds = Mathf.Max(timeLeftInSeconds, 0);

            int minutes = Mathf.FloorToInt(timeLeftInSeconds / 60);
            int seconds = Mathf.FloorToInt(timeLeftInSeconds % 60);
            timeText.text = $"TIME LEFT: {minutes:00}:{seconds:00}";
        }
    }

    public void StopTimer() => isRunning = false;
    public void ResumeTimer() => isRunning = true;

    public void ResetTimer(float newTime)
    {
        timeLeftInSeconds = newTime;
        isRunning = true;
    }
}

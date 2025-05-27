using UnityEngine;
using UnityEngine.UI;

public class QuickTimeEvent2 : MonoBehaviour 
{
    [Header("QTE Visuals")]
    public GameObject qteUI;
    public Slider timerSlider;

    [Header("Timing Settings")]
    public float timerDuration = 15f;
    public int requiredPresses = 30;
    public KeyCode inputKey = KeyCode.A;

    [Header("References")]
    public PlayerMovement movementScript; 

    private float elapsedTime = 0f;
    private bool isRunning = false;
    private int pressCount = 0;

    void OnEnable()
    {
        StartQTE();
    }

    void StartQTE()
    {
        qteUI.SetActive(true);
        elapsedTime = 0f;
        pressCount = 0;
        isRunning = true;
        timerSlider.value = 1f;

        if (movementScript != null)
        {
            movementScript.blockAInput = true; //block "A" movement while QTE is running
            Debug.Log("A key movement blocked (QTE started)");
        }
    }

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;

        float remaining = Mathf.Clamp01(1 - (elapsedTime / timerDuration));
        timerSlider.value = remaining;

        if (Input.GetKeyDown(inputKey))
        {
            pressCount++;
            Debug.Log("Pressed A: " + pressCount);

            if (pressCount >= requiredPresses)
            {
                Debug.Log("QTE Success!");
                EndQTE();
            }
        }

        if (elapsedTime >= timerDuration)
        {
            Debug.Log(pressCount >= requiredPresses ? "QTE Success!" : "QTE Failed!");
            EndQTE();
        }
    }

    void EndQTE()
    {
        isRunning = false;
        qteUI.SetActive(false);

        gameObject.SetActive(false); // Triggers OnDisable()
    }

    void OnDisable()
    {
        //Always re-enable A movement when this QTE is disabled
        if (movementScript != null)
        {
            movementScript.blockAInput = false;
            Debug.Log("A key movement re-enabled (QTE ended)");
        }
    }
}
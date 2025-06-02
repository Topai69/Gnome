using UnityEngine;
using UnityEngine.UI;

public class DualKeyQuickTimeEvent : MonoBehaviour
{
    [Header("QTE Visuals")]
    public GameObject qteUI;
    public Slider timerSlider; 

    [Header("Timing Settings")]
    public float timerDuration = 15f; 

    [Header("Input Keys")]
    public KeyCode keyOne = KeyCode.A;
    public KeyCode keyTwo = KeyCode.LeftArrow;

    [Header("References")]
    public LeftSinkValveInteractable valveScript; 
    public PlayerMovement movementScript;
    [SerializeField] public PlayerMovement plr;

    private float elapsedTime = 0f;
    private bool isRunning = false;

    void OnEnable()
    {
        StartQTE();
    }

    void StartQTE()
    {
        qteUI.SetActive(true);
        elapsedTime = 0f;
        isRunning = true;

        if (timerSlider != null)
            timerSlider.value = 1f;

        if (movementScript != null)
        {
            movementScript.blockAInput = true;
            Debug.Log("A movement blocked for dual key QTE");
        }
    }

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;
        float remaining = Mathf.Clamp01(1 - (elapsedTime / timerDuration));

        if (timerSlider != null)
            timerSlider.value = remaining;

        // Check if both keys are pressed simultaneously
        bool success = (Input.GetKey(keyOne) && Input.GetKeyDown(keyTwo) || (Input.GetKeyDown(KeyCode.Joystick1Button0))) || (Input.GetKeyDown(keyOne) && (Input.GetKey(keyTwo) || Input.GetKeyDown(KeyCode.Joystick1Button0)));

        if (success)
        {
            Debug.Log("QTE Success: Left + A pressed");
            Complete(success: true);
        }

        if (elapsedTime >= timerDuration)
        {
            Debug.Log("QTE Failed: Time ran out");
            Complete(success: false);
        }
    }

    void Complete(bool success)
    {
        isRunning = false;
        qteUI.SetActive(false);
        gameObject.SetActive(false); // Triggers OnDisable()

        if (movementScript != null)
            movementScript.blockAInput = false;

        if (success && valveScript != null)
            valveScript.FinishValveInteraction();
    }

    void OnDisable()
    {
        if (movementScript != null)
        {
            movementScript.blockAInput = false;
            Debug.Log("A movement re-enabled (QTE ended)");
        }
    }
}
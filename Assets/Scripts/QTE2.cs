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

    [Header("References")]
    public PlayerMovement movementScript;

    private float elapsedTime = 0f;
    private bool isRunning = false;
    private int pressCount = 0;

    private enum QTEStage { A_Spam, B_Spam, APlusB }
    [SerializeField] private QTEStage currentStage = QTEStage.A_Spam;

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
            movementScript.blockAInput = true;
            movementScript.blockBInput = true;
            movementScript.blockJump = true;
            Debug.Log("A key movement and jumping blocked (QTE started)");
        }
    }

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;
        timerSlider.value = Mathf.Clamp01(1 - (elapsedTime / timerDuration));

        switch (currentStage)
        {
            case QTEStage.A_Spam:
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.JoystickButton0))
                    IncrementPress("A");
                break;

            case QTEStage.B_Spam:
                if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.JoystickButton1))
                    IncrementPress("B");
                break;

            case QTEStage.APlusB:
                bool aHeld = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.JoystickButton0);
                bool bHeld = Input.GetKey(KeyCode.B) || Input.GetKey(KeyCode.JoystickButton1);
                if (aHeld && bHeld)
                {
                    Debug.Log("Pressed A + B simultaneously");
                    EndQTE();
                }
                break;
        }

        if (elapsedTime >= timerDuration)
        {
            Debug.Log("QTE Failed (Timer)");
            EndQTE();
        }
    }

    void IncrementPress(string keyLabel)
    {
        pressCount++;
        Debug.Log($"Pressed {keyLabel}: {pressCount}/{requiredPresses}");

        if (pressCount >= requiredPresses)
        {
            Debug.Log($"QTE {keyLabel} Success!");
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
        if (movementScript != null)
        {
            movementScript.blockAInput = false;
            movementScript.blockBInput = false;
            movementScript.blockJump = false;
            Debug.Log("A key movement and jumping re-enabled (QTE ended)");
        }
    }

    public void SetStageToBSpam() => currentStage = QTEStage.B_Spam;
    public void SetStageToAPlusB() => currentStage = QTEStage.APlusB;
}

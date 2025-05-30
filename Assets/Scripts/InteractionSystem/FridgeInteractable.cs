using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FridgeInteractable : InteractableBase
{
    [SerializeField] private TaskManager taskManager;
    [HideInInspector] public ScoreScript ScoreScript;

    [Header("QTE Visuals")]
    public GameObject qteUI;
    public Slider timerSlider;

    [Header("Timing Settings")]
    public float timerDuration = 15f;

    private float elapsedTime = 0f;
    private bool isRunning = false;
    private int pressCount = 0;
    private int requiredPresses = 10;

    private void Start()
    {
        ScoreScript = FindAnyObjectByType<ScoreScript>();
    }

    public override void OnInteract()
    {
        base.OnInteract();
        Debug.Log("Interacted with fridge");

        qteUI.SetActive(true);
        elapsedTime = 0f;
        pressCount = 0;
        isRunning = true;

        timerSlider.value = 1f;
    }

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;

        float remaining = Mathf.Clamp01(1 - (elapsedTime / timerDuration));
        timerSlider.value = remaining;
        
        // Player input check
        if (Input.GetKeyDown(KeyCode.A))
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

        if (pressCount >= requiredPresses)
        {
            Finish();
        }
        else
        {
            Debug.Log("Player can retry.");
        }
    }

    private void Finish()
    {
        Animation anim = GetComponentInParent<Animation>();
        if (anim != null)
        {
            anim.Play("Fridge");
        }
        else
        {
            Debug.LogWarning("No Animation component found on parent.");
        }

        if (taskManager != null)
        {
            taskManager.CompleteTask(0);
        }

        if (ScoreScript != null)
        {
            ScoreScript.Score += 20;
        }

        GetComponent<FridgeInteractable>().enabled = false; //turn of the script, since it's no longer needed
        gameObject.layer = 6; //change layer so that the object was no longer interactable
    }
}

using UnityEngine;
using UnityEngine.UI;

public class LeftSinkValveInteractable : InteractableBase
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

    private void Start()
    {
        ScoreScript = FindAnyObjectByType<ScoreScript>();
    }

    public override void OnInteract()
    {
        base.OnInteract();
        Debug.Log("Interacted with left sink valve");

        StartQTE();
    }

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;
        float remaining = Mathf.Clamp01(1 - (elapsedTime / timerDuration));
        if (timerSlider != null)
            timerSlider.value = remaining;

        if (elapsedTime >= timerDuration)
        {
            Debug.Log("QTE Timeout");
            EndQTE();
        }
    }

    void StartQTE()
    {
        if (qteUI != null)
            qteUI.SetActive(true);

        elapsedTime = 0f;
        isRunning = true;

        if (timerSlider != null)
            timerSlider.value = 1f;
    }

    void EndQTE()
    {
        isRunning = false;

        if (qteUI != null)
            qteUI.SetActive(false);

        FinishValveInteraction();
    }

    public void FinishValveInteraction()
    {
        //anim.Play("Rotating");
        (transform.parent.gameObject.GetComponent<Animation>()).Play("LeftSinkValve");

        if (taskManager != null)
        {
            taskManager.CompleteTask(2);
        }

        if (ScoreScript != null)
        {
            ScoreScript.Score += 10;
        }

        GetComponent<LeftSinkValveInteractable>().enabled = false; //turn of the script, since it's no longer needed
        gameObject.layer = 6;
    }
}
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

    public float elapsedTime = 0f;
    public bool isRunning = false;
    private bool flag = false;
    [SerializeField] private float timer = 3f;
    [SerializeField] private GameObject vfx;
    [SerializeField] private GameObject mapIcon;
    [SerializeField] public AudioClip voiceLine;
    [SerializeField] private WaterDripping water;

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

    public void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;
        float remaining = Mathf.Clamp01(1 - (elapsedTime / timerDuration));
        if (timerSlider != null)
            timerSlider.value = remaining;

        if (elapsedTime >= timerDuration)
        {
            Debug.Log("QTE Timeout");
            //EndQTE();
        }
    }

    private void FixedUpdate()
    {
        if (flag)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                vfx.SetActive(false);
                GetComponent<LeftSinkValveInteractable>().enabled = false;
            }
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

   /* public void EndQTE()
    {
        isRunning = false;

        if (qteUI != null)
            qteUI.SetActive(false);

        FinishValveInteraction();
    }*/

    public void FinishValveInteraction()
    {
        Debug.Log("working");
        //anim.Play("Rotating");
        transform.parent.gameObject.GetComponent<Animation>().Play("Sink");

        if (taskManager != null)
        {
            taskManager.CompleteTask(2);
        }

        if (ScoreScript != null)
        {
            ScoreScript.Score += 10;
        }

        
        gameObject.layer = 6;
        GetComponent<AudioSource>().PlayOneShot(voiceLine);
        vfx.SetActive(true);
        flag = true;
        mapIcon.SetActive(false);
        water.Stop();
    }
}
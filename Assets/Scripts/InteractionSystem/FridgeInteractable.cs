using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class FridgeInteractable : InteractableBase
{
    [SerializeField] private TaskManager taskManager;
    [HideInInspector] public ScoreScript ScoreScript;
    [SerializeField] public QuickTimeEvent2 quickTime;


    [Header("QTE Visuals")]
    public GameObject qteUI;
    public Slider timerSlider;
    public VisualEffectAsset effect;
    [SerializeField] private Image aButtonImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite pressedSprite;

    [Header("Timing Settings")]
    public float timerDuration = 15f;

    private string names;
    private float elapsedTime = 0f;
    private bool isRunning = false;
    private int pressCount = 0;
    private int requiredPresses = 10;

    private AnimationTest animController;

    private void Start()
    {
        ScoreScript = FindAnyObjectByType<ScoreScript>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            animController = player.GetComponentInChildren<AnimationTest>();
        }
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
       
        if (animController != null)
        {
            animController.StartPushingAnimation();
        }
    }

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;

        float remaining = Mathf.Clamp01(1 - (elapsedTime / timerDuration));
        timerSlider.value = remaining;
    
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.JoystickButton0))
    {
        if (aButtonImage != null && pressedSprite != null)
        aButtonImage.sprite = pressedSprite; 

        pressCount++;
        Debug.Log("Pressed A: " + pressCount);

        if (pressCount >= requiredPresses)
        {
        Debug.Log("QTE Success!");
        EndQTE();
        }
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.JoystickButton0))
        {
        if (aButtonImage != null && normalSprite != null)
        aButtonImage.sprite = normalSprite;
    }
    }


    void EndQTE()
    {
        isRunning = false;
        qteUI.SetActive(false);

        if (animController != null)
        {
            animController.StopPushingAnimation();
        }

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
        transform.parent.gameObject.GetComponent<Animation>().Play("Fridge");
        /*if (anim != null)
        {
            //anim.Play("Fridge");
            transform.parent.gameObject.GetComponent<Animation>().Play("Fridge");
        }
        else
        {
            Debug.LogWarning("No Animation component found on parent.");
        }
        */

        if (taskManager != null)
        {
            taskManager.CompleteTask(0);
        }

        if (ScoreScript != null)
        {
            ScoreScript.Score += 20;
        }

       
        GetComponent<FridgeInteractable>().enabled = false; //turn of the script, since it's no longer needed
        gameObject.layer = 6; 
    }
}

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
    [SerializeField] private Image bButtonImage;
    [SerializeField] private Image abButtonImage;

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private Sprite bNormalSprite;
    [SerializeField] private Sprite bPressedSprite;
    [SerializeField] private Sprite abNormalSprite;
    [SerializeField] private Sprite abPressedSprite;

    [Header("Timing Settings")]
    public float timerDuration = 15f;

    private bool flag = false;
    private float elapsedTime = 0f;
    private bool isRunning = false;
    private int pressCount = 0;
    private int requiredPresses = 10;
    [SerializeField] private float timer = 3f;
    [SerializeField] GameObject vfx;
    [SerializeField] GameObject mapIcon;

    private AnimationTest animController;

    private enum QTEStage { A, B, AB }
    private QTEStage currentStage = QTEStage.A;

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
        currentStage = QTEStage.A;

        timerSlider.value = 1f;
        UpdateButtonUI(QTEStage.A);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.blockJump = true;
            }
        }

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

        switch (currentStage)
        {
            case QTEStage.A:
                HandleSingleKey(KeyCode.A, KeyCode.JoystickButton0, aButtonImage, normalSprite, pressedSprite, QTEStage.B);
                break;
            case QTEStage.B:
                HandleSingleKey(KeyCode.B, KeyCode.JoystickButton1, bButtonImage, bNormalSprite, bPressedSprite, QTEStage.AB);
                break;
            case QTEStage.AB:
                HandleDualKey(KeyCode.A, KeyCode.B, KeyCode.JoystickButton0, KeyCode.JoystickButton1, abButtonImage, abNormalSprite, abPressedSprite);
                break;
        }

        if (elapsedTime >= timerDuration)
        {
            Debug.Log("QTE Failed!");
            EndQTE();
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
                GetComponent<FridgeInteractable>().enabled = false;
            }
        }
    }

    void HandleSingleKey(KeyCode key, KeyCode joystickKey, Image buttonImage, Sprite normal, Sprite pressed, QTEStage nextStage)
    {
        if (Input.GetKeyDown(key) || Input.GetKeyDown(joystickKey))
        {
            if (buttonImage != null && pressed != null)
                buttonImage.sprite = pressed;

            pressCount++;
            Debug.Log($"{key} Pressed: {pressCount}");

            if (pressCount >= requiredPresses)
            {
                pressCount = 0;
                currentStage = nextStage;
                UpdateButtonUI(nextStage);

                if (currentStage == QTEStage.B)
                    quickTime.SetStageToBSpam();
                else if (currentStage == QTEStage.AB)
                    quickTime.SetStageToAPlusB();
            }
        }

        if (Input.GetKeyUp(key) || Input.GetKeyUp(joystickKey))
        {
            if (buttonImage != null && normal != null)
                buttonImage.sprite = normal;
        }
    }

    void HandleDualKey(KeyCode key1, KeyCode key2, KeyCode joystick1, KeyCode joystick2, Image buttonImage, Sprite normal, Sprite pressed)
    {
        bool aDown = Input.GetKey(key1) || Input.GetKey(joystick1);
        bool bDown = Input.GetKey(key2) || Input.GetKey(joystick2);

        if (aDown && bDown)
        {
            if (buttonImage != null && pressed != null)
                buttonImage.sprite = pressed;

            Debug.Log("Pressed A + B successfully!");
            EndQTE();
        }
        else
        {
            if (buttonImage != null && normal != null)
                buttonImage.sprite = normal;
        }
    }

    void UpdateButtonUI(QTEStage stage)
    {
        if (aButtonImage != null) aButtonImage.gameObject.SetActive(stage == QTEStage.A);
        if (bButtonImage != null) bButtonImage.gameObject.SetActive(stage == QTEStage.B);
        if (abButtonImage != null) abButtonImage.gameObject.SetActive(stage == QTEStage.AB);
    }

    void EndQTE()
    {
        isRunning = false;
        qteUI.SetActive(false);

        if (currentStage == QTEStage.AB)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerMovement movement = player.GetComponent<PlayerMovement>();
                if (movement != null)
                {
                    movement.blockJump = true;
                    movement.blockAInput = true;
                    movement.blockBInput = true;
                }
            }

            Finish();
        }
        else
        {
            Debug.Log("QTE Failed or interrupted.");
            if (animController != null)
            {
                animController.StopPushingAnimation();
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerMovement movement = player.GetComponent<PlayerMovement>();
                if (movement != null)
                {
                    movement.blockJump = false;
                }
            }
        }
    }

    private void Finish()
    {
        Animation anim = GetComponentInParent<Animation>();
        transform.parent.gameObject.GetComponent<Animation>().Play("Fridge");
        float fridgeAnimationDuration = transform.parent.gameObject.GetComponent<Animation>()["Fridge"].length;
        StartCoroutine(StopPushingAfterFridgeAnimation(fridgeAnimationDuration));

        if (taskManager != null)
        {
            taskManager.CompleteTask(0);
        }

        if (ScoreScript != null)
        {
            ScoreScript.Score += 20;
        }


        vfx.SetActive(true);
        flag = true;
        gameObject.layer = 6;
        mapIcon.SetActive(false); 
    }

    private IEnumerator StopPushingAfterFridgeAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.blockJump = false;
                movement.blockAInput = false;
                movement.blockBInput = false;
            }
        }

        if (animController != null)
        {
            animController.StopPushingAnimation();
        }
    }
}

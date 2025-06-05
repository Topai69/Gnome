using UnityEngine;
using UnityEngine.UI;

public class QuickTimeEvent : MonoBehaviour
{
    [Header("QTE Visuals")]
    public GameObject qteUI;
    public Slider timerSlider;
    public RectTransform cursor;
    public RectTransform successZone;

    [Header("Timing Settings")]
    public float timerDuration = 15f;
    public float cursorTravelTime = 2f;

    [Header("References")]
    public HeaterInteractable heaterScript;
    [SerializeField] public PlayerMovement plr;

    private float elapsedTime = 0f;
    private bool isRunning = false;
    private bool success = false;

    private int currentStage = 0;
    private const int maxStages = 3;

    private Vector3 cursorStartPos;
    private Vector3 cursorEndPos;

    private RectTransform backgroundBar;

    private void OnEnable()
    {
        backgroundBar = qteUI.transform.Find("BackgroundBar").GetComponent<RectTransform>();
        StartQTEStage(0);
    }

    void StartQTEStage(int stage)
    {
        qteUI.SetActive(true);
        elapsedTime = 0f;
        isRunning = true;
        success = false;

        if (timerSlider != null)
            timerSlider.value = 1f;

        // Calculate UI width
        float width = backgroundBar.rect.width;

        cursorStartPos = new Vector3(-width / 2f, cursor.localPosition.y, 0);
        cursorEndPos = new Vector3(width / 2f, cursor.localPosition.y, 0);
        cursor.localPosition = cursorStartPos;

        // Randomize green thing position
        float zoneWidth = stage == 0 ? 100 : (stage == 1 ? 60 : 30);
        successZone.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, zoneWidth);

        float minX = -width / 2f + zoneWidth / 2f;
        float maxX = width / 2f - zoneWidth / 2f;
        float randomX = Random.Range(minX, maxX);
        successZone.localPosition = new Vector3(randomX, successZone.localPosition.y, 0);

        if (plr != null) {
            plr.blockAInput = true;
            plr.blockJump = true; 
        }
    }

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;
        float remaining = Mathf.Clamp01(1 - (elapsedTime / timerDuration));

        if (timerSlider != null)
            timerSlider.value = remaining;

        float cursorProgress = Mathf.PingPong(elapsedTime / cursorTravelTime, 1f);
        cursor.localPosition = Vector3.Lerp(cursorStartPos, cursorEndPos, cursorProgress);

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (IsCursorInSuccessZone())
            {
                Debug.Log($"QTE {currentStage + 1} Success!");
                ProceedToNextStage();
            }
            else
            {
                Debug.Log("QTE Failed! Pressed outside the green zone.");
                EndQTE(false);
            }
        }

        if (elapsedTime >= timerDuration)
        {
            Debug.Log("QTE Failed! Timer ran out.");
            EndQTE(false);
        }
    }

    bool IsCursorInSuccessZone()
    {
        float cursorX = cursor.position.x;
        float zoneMin = successZone.position.x - (successZone.rect.width / 2);
        float zoneMax = successZone.position.x + (successZone.rect.width / 2);
        return cursorX >= zoneMin && cursorX <= zoneMax;
    }

    void ProceedToNextStage()
    {
        currentStage++;
        if (currentStage < maxStages)
        {
            StartQTEStage(currentStage);
        }
        else
        {
            EndQTE(true);
        }
    }

    void EndQTE(bool wasSuccessful)
    {
        isRunning = false;
        qteUI.SetActive(false);
        gameObject.SetActive(false);

        if (plr != null) {
            plr.blockAInput = false;
            plr.blockJump = false; 
        }

        // Only complete heater if QTE was successful
        if (wasSuccessful && heaterScript != null)
        {
            heaterScript.FinishHeaterInteraction();
        }
        else
        {
            Debug.Log("QTE failed or no Heater script reference, allowing retry.");
            if (heaterScript != null)
            {
                Collider col = heaterScript.GetComponent<Collider>();
                if (col != null) col.enabled = true;
                heaterScript.hasInteracted = false;
            }
        }

        currentStage = 0;
    }
}

using System.Collections;
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

    private Vector3 cursorStartPos;
    private Vector3 cursorEndPos;

    void OnEnable()
    {
        StartQTE();
    }

    void StartQTE()
    {
        qteUI.SetActive(true);
        elapsedTime = 0f;
        isRunning = true;
        success = false;

        timerSlider.value = 1f;

        RectTransform background = qteUI.transform.Find("BackgroundBar").GetComponent<RectTransform>();
        float width = background.rect.width;

        cursorStartPos = new Vector3(-width / 2f, cursor.localPosition.y, 0);
        cursorEndPos = new Vector3(width / 2f, cursor.localPosition.y, 0);
        cursor.localPosition = cursorStartPos;
        plr.blockAInput = true;
    }

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;

        float remaining = Mathf.Clamp01(1 - (elapsedTime / timerDuration));
        timerSlider.value = remaining;

        float cursorProgress = Mathf.PingPong(elapsedTime / cursorTravelTime, 1f);
        cursor.localPosition = Vector3.Lerp(cursorStartPos, cursorEndPos, cursorProgress);

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            if (IsCursorInSuccessZone())
            {
                Debug.Log("QTE Success!");
                success = true;
            }
            else
            {
                Debug.Log("QTE Failed! Pressed outside the green zone.");
                success = false;
            }

            EndQTE();
        }

        if (elapsedTime >= timerDuration)
        {
            Debug.Log("QTE Failed! Timer ran out.");
            success = false;
            EndQTE();
        }
    }

    bool IsCursorInSuccessZone()
    {
        float cursorX = cursor.position.x;
        float zoneMin = successZone.position.x - (successZone.rect.width / 2);
        float zoneMax = successZone.position.x + (successZone.rect.width / 2);
        return cursorX >= zoneMin && cursorX <= zoneMax;
    }

    void EndQTE()
    {
        isRunning = false;
        qteUI.SetActive(false);
        gameObject.SetActive(false);

        // Only complete heater if QTE was successful
        if (success && heaterScript != null)
        {
            heaterScript.FinishHeaterInteraction();
        }
        else
        {
            Debug.Log("QTE failed or no Heater script reference, allowing retry.");

            if (heaterScript != null)
            {
                Collider col = heaterScript.GetComponent<Collider>();
                if (col != null)
                {
                    col.enabled = true;
                }
                heaterScript.hasInteracted = false;
            }

        }
        plr.blockAInput = false;
    }
}

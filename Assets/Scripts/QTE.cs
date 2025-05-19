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

    private float elapsedTime = 0f;
    private bool isRunning = false;

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

        timerSlider.value = 1f;

        RectTransform background = qteUI.transform.Find("BackgroundBar").GetComponent<RectTransform>();
        float width = background.rect.width;

        cursorStartPos = new Vector3(-width / 2f, cursor.localPosition.y, 0);
        cursorEndPos = new Vector3(width / 2f, cursor.localPosition.y, 0);
        cursor.localPosition = cursorStartPos;
    }

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;

        float remaining = Mathf.Clamp01(1 - (elapsedTime / timerDuration));
        timerSlider.value = remaining;

        // Cursor movement 
        float cursorProgress = Mathf.PingPong(elapsedTime / cursorTravelTime, 1f);
        cursor.localPosition = Vector3.Lerp(cursorStartPos, cursorEndPos, cursorProgress);

        // Player input check
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (IsCursorInSuccessZone())
            {
                Debug.Log("QTE Success!");
                EndQTE();
            }
        }

        if (elapsedTime >= timerDuration)
        {
            Debug.Log("QTE Failed!");
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
    }
}

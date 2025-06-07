using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuickTimeEvent2 : MonoBehaviour
{
    [Header("QTE Visuals")]
    public GameObject qteUI;
    public Slider timerSlider;

    [Header("Timing Settings")]
    public float timerDuration = 15f;
    public int requiredPresses = 30;
    public KeyCode inputKey = KeyCode.A;

    [Header("References")]
    public PlayerMovement movementScript;
    public FridgeInteractable interactable;

    private float elapsedTime = 0f;
    private bool isRunning = false;
    private int pressCount = 0;
    private int pressCount2 = 0;
    private bool flag1 = false;
    private bool flag2 = false;

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

        movementScript.blockAInput = true;
        movementScript.blockJump = true;
        Debug.Log("A key movement and jumping blocked (QTE started)");
    }

    void Update()
    {
        if (!isRunning) return;
        movementScript.MovePlayer();
        if (Input.GetKeyDown(inputKey) || Input.GetKeyDown(KeyCode.JoystickButton0) && !flag1)
        {
            pressCount++;
            Debug.Log("Pressed A: " + pressCount);

        }
        if (pressCount >= requiredPresses && !flag2)
        {
            flag1 = true;
            if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                pressCount2++;
                Debug.Log("Pressed B: " + pressCount2);
            }
        }
        if (pressCount2 >= requiredPresses - 5)
        {
            flag2 = true;
            if ((Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.B)) 
                || (Input.GetKeyDown(KeyCode.Joystick1Button0) && Input.GetKeyDown(KeyCode.Joystick1Button1)))
            {
                Debug.Log("QTE Success");
                EndQTE();
            }
        }
        elapsedTime += Time.deltaTime;
        float remaining = Mathf.Clamp01(1 - (elapsedTime / timerDuration));
        timerSlider.value = remaining;
    }


    void EndQTE()
    {
        isRunning = false;
        qteUI.SetActive(false);
        gameObject.SetActive(false); // Triggers OnDisable()
    }

    void OnDisable()
    {
     
        movementScript.blockAInput = false;
        movementScript.blockJump = false; 
        Debug.Log("A key movement and jumping re-enabled (QTE ended)");
        interactable.Finish();
    }
}
using UnityEngine;
using UnityEngine.UI;

public class DualKeyQuickTimeEvent : MonoBehaviour
{
    [Header("QTE Visuals")]
    public GameObject qteUI;
    public Slider timerSlider; 

    [Header("Timing Settings")]
    public float timerDuration = 15f; 
    
    [Header("Input Keys")]
    public KeyCode keyOne = KeyCode.A;
    public KeyCode keyTwo = KeyCode.LeftArrow;

    [SerializeField] LeftSinkValveInteractable valve;

    [Header("References")]
    //public LeftSinkValveInteractable valveScript; 
    public PlayerMovement movementScript;
    //[SerializeField] public PlayerMovement plr;

    bool success;

   // private float elapsedTime = 0f;
   // private bool isRunning = false;


    void OnEnable()
    {
        StartQTE();

    }

    void StartQTE()
    {
        qteUI.SetActive(true);
        valve.elapsedTime = 0f;
        valve.isRunning = true;

        if (timerSlider != null)
            timerSlider.value = 1f;

        if (movementScript != null)
        {
            movementScript.blockAInput = true;
            movementScript.blockJump = true;  
            Debug.Log("A movement and jumping blocked for dual key QTE");
        }
    }

    void Update()
    {
        if (!valve.isRunning) return;

        valve.elapsedTime += Time.deltaTime;
        float remaining = Mathf.Clamp01(1 - (valve.elapsedTime / valve.timerDuration));

        if (timerSlider != null)
            timerSlider.value = remaining;
        
        // Check if both keys are pressed simultaneously

        success = ((Input.GetKey(keyOne) && Input.GetKeyDown(keyTwo) || (Input.GetKeyDown(KeyCode.Joystick1Button0)
            || (Input.GetKeyDown(keyOne) && (Input.GetKey(keyTwo) || (Input.GetKeyDown(KeyCode.Joystick1Button0)) || (Input.GetKeyDown(KeyCode.Joystick1Button0)))))));
        if (success)
        {
            Debug.Log("QTE Success: Left + A pressed");
            
            Complete();

        }

        if (valve.elapsedTime >= valve.timerDuration)
        {
            Debug.Log("QTE Failed: Time ran out");
            Complete();
        }
    }

    void Complete()
    {
        valve.isRunning = false;
        qteUI.SetActive(false);
   
        if (movementScript != null) {
            movementScript.blockAInput = false;
            movementScript.blockJump = false;  
        }

        if (success) valve.FinishValveInteraction();
         
        gameObject.SetActive(false); // Triggers OnDisable()
    }

    void OnDisable()
    {
        if (movementScript != null)
        {
            movementScript.blockAInput = false;
            movementScript.blockJump = false;  
            Debug.Log("A movement and jumping re-enabled (QTE ended)");
        }
    }
}
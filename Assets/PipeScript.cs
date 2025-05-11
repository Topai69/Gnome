using UnityEngine;
using UnityEngine.UI;

public class PipeFixer : MonoBehaviour
{
    [Header("Pipe Models")]
    public GameObject brokenPipe; // current broken pipe model
    public GameObject fixedPipe; 

    [Header("Leak Effect")]
    public GameObject leakEffect; // effect of water

    [Header("UI")]
    public GameObject fixUIPrompt; // UI prompt to hold E
    public Slider fixProgressBar;

    [Header("Fix Settings")]
    public float fixTime = 3f; // seconds needed to hold E
    private float fixTimer = 0f;

    private bool playerInRange = false;
    private bool fixedAlready = false;

    void Start()
    {
        fixUIPrompt.SetActive(false);
        fixProgressBar.value = 0;
    }

    void Update()
    {
        if (playerInRange && !fixedAlready)
        {
            fixUIPrompt.SetActive(true);

            if (Input.GetKey(KeyCode.E))
            {
                fixTimer += Time.deltaTime;
                fixProgressBar.value = fixTimer / fixTime;

                if (fixTimer >= fixTime)
                {
                    FixPipe();
                }
            }
            else
            {
                fixTimer -= Time.deltaTime * 2; // goes down when not holding
                fixTimer = Mathf.Clamp(fixTimer, 0, fixTime);
                fixProgressBar.value = fixTimer / fixTime;
            }
        }
        else
        {
            fixUIPrompt.SetActive(false);
        }
    }

    private void FixPipe()
    {
        fixedAlready = true;

        // Switch pipe models (transition)
        brokenPipe.SetActive(false);
        fixedPipe.SetActive(true);

        // Stop leak
        if (leakEffect != null)
            leakEffect.SetActive(false);

        // Hide UI
        fixUIPrompt.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
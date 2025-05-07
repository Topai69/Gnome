using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class KeypadInteraction : MonoBehaviour
{
    [Header("Keypad Settings")]
    [SerializeField] private GameObject keypadUI;
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private string correctCode = "1234";
    [SerializeField] private float interactionDistance = 2f;
    
    [Header("References")]
    [SerializeField] private Transform player;
    
    private string currentInput = "";
    private bool isKeypadOpen = false;
    private bool isNearLaptop = false;

    private void Start()
    {
        if (keypadUI != null)
            keypadUI.SetActive(false);
    }

    private void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            isNearLaptop = distance <= interactionDistance;
        }
        if (isNearLaptop && Input.GetKeyDown(KeyCode.E) && !isKeypadOpen)
        {
            OpenKeypad();
        }
        else if (isKeypadOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseKeypad();
        }
    }

    private void OpenKeypad()
    {
        isKeypadOpen = true;
        keypadUI.SetActive(true);
        currentInput = "";
        UpdateDisplay();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseKeypad()
    {
        isKeypadOpen = false;
        keypadUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void AddNumber(string number)
    {
        if (currentInput.Length < 4)
        {
            currentInput += number;
            UpdateDisplay();
        }
    }

    public void ClearInput()
    {
        currentInput = "";
        UpdateDisplay();
    }

    public void SubmitCode()
    {
        if (currentInput == correctCode)
        {
            Debug.Log("gvcci");
        }
        else
        {
            Debug.Log("NOOOOO");
            currentInput = "";
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        if (displayText != null)
        {
            displayText.text = currentInput;
        }
    }
} 
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class KeypadInteraction : InteractableBase
{
    [Header("Keypad Settings")]
    [SerializeField] private GameObject keypadUI;
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private string correctCode = "1234";
    
    private string currentInput = "";
    private bool isKeypadOpen = false;

    private void Start()
    {
        if (keypadUI != null)
            keypadUI.SetActive(false);
    }

    public override void OnInteract()
    {
        base.OnInteract();
        if (!isKeypadOpen)
        {
            OpenKeypad();
        }
    }

    private void Update()
    {
        if (isKeypadOpen && Input.GetKeyDown(KeyCode.Escape))
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
            Debug.Log("NOOOO");
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
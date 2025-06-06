using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintSystem : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject hintPromptPanel;
    [SerializeField] private TextMeshProUGUI hintPromptText;
    
    [Header("Hint Arrows")]
    [SerializeField] private GameObject[] taskArrowGroups; 
    
    private bool hintsActive = false;
    private int currentTaskIndex = -1;
    
    void Start()
    {
        if (hintPromptPanel != null)
            hintPromptPanel.SetActive(false);
            
        HideAllArrows();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && currentTaskIndex >= 0)
        {
            hintsActive = !hintsActive;
            ToggleHintArrows(hintsActive);
        }
    }
    
    public void ShowHintPrompt(int taskIndex)
    {
        if (taskIndex >= 0 && taskIndex < taskArrowGroups.Length)
        {
            currentTaskIndex = taskIndex;
            
            if (hintPromptPanel != null)
            {
                hintPromptPanel.SetActive(true);
                if (hintPromptText != null)
                    hintPromptText.text = "Press H for a hint";
            }
        }
    }
    
    public void HideHintPrompt()
    {
        if (hintPromptPanel != null)
            hintPromptPanel.SetActive(false);

        hintsActive = false;
        ToggleHintArrows(false);
    }

    private void ToggleHintArrows(bool show)
    {
        HideAllArrows();
        
        if (show && currentTaskIndex >= 0 && currentTaskIndex < taskArrowGroups.Length && taskArrowGroups[currentTaskIndex] != null)
        {
            taskArrowGroups[currentTaskIndex].SetActive(true);
        }
    }
    
    private void HideAllArrows()
    {
        foreach (GameObject arrowGroup in taskArrowGroups)
        {
            if (arrowGroup != null)
                arrowGroup.SetActive(false);
        }
    }
    
    public void OnTaskCompleted(int taskIndex)
    {
        if (taskIndex == currentTaskIndex)
        {
            hintsActive = false;
            HideHintPrompt();
        }
    }
}
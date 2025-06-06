using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; 

public class HintSystem : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject hintPromptPanel;
    [SerializeField] private TextMeshProUGUI hintPromptText;
    
    [Header("Task Hint Selection UI")]
    [SerializeField] private GameObject taskSelectionPanel;
    [SerializeField] private Button[] taskSelectionButtons;
    [SerializeField] private string[] taskNames;
    
    [Header("Hint Arrows")]
    [SerializeField] private GameObject[] taskArrowGroups; 
    
    [Header("External References")]
    [SerializeField] private PauseMenu pauseMenu; 
    
    private bool hintsActive = false;
    private int currentTaskIndex = -1;
    private bool isTaskSelectionOpen = false;
    
    void Start()
    {
        HideAllArrows();
        SetupTaskButtons();
        
        if (hintPromptPanel != null)
            hintPromptPanel.SetActive(true);
            

        if (taskSelectionPanel != null)
            taskSelectionPanel.SetActive(false);
    }
    
    void SetupTaskButtons()
    {
        if (taskSelectionButtons == null || taskSelectionButtons.Length == 0)
            return;
            
        for (int i = 0; i < taskSelectionButtons.Length; i++)
        {
            int index = i; 
            
            if (taskSelectionButtons[i] != null)
            {
                if (i < taskNames.Length)
                {
                    TextMeshProUGUI buttonText = taskSelectionButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                    if (buttonText != null)
                        buttonText.text = taskNames[i];
                }
                
                taskSelectionButtons[i].onClick.AddListener(() => SelectTask(index));
            }
        }
    }
    
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.JoystickButton3)) 
        {
            ToggleTaskSelection();
        }
    }
    
    private void ToggleTaskSelection()
    {
        if (isTaskSelectionOpen)
        {
            taskSelectionPanel.SetActive(false);
            isTaskSelectionOpen = false;
            
            if (hintsActive)
            {
                hintsActive = false;
                ToggleHintArrows(false);
            }

            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            taskSelectionPanel.SetActive(true);
            isTaskSelectionOpen = true;
            
            if (taskSelectionButtons != null && taskSelectionButtons.Length > 0 && taskSelectionButtons[0] != null)
            {
                EventSystem.current.SetSelectedGameObject(taskSelectionButtons[0].gameObject);
            }
        }
    }
    
    public void SelectTask(int taskIndex)
    {
        if (taskIndex >= 0 && taskIndex < taskArrowGroups.Length)
        {
            currentTaskIndex = taskIndex;

            hintsActive = true;
            ToggleHintArrows(true);

            if (taskSelectionPanel != null)
            {
                taskSelectionPanel.SetActive(false);
                isTaskSelectionOpen = false;
            }
      
            EventSystem.current.SetSelectedGameObject(null);
        }
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
        if (taskIndex == currentTaskIndex && hintsActive)
        {
            hintsActive = false;
            ToggleHintArrows(false);
        }
    }
    
    public void CloseTaskSelection()
    {
        if (isTaskSelectionOpen)
        {
            taskSelectionPanel.SetActive(false);
            isTaskSelectionOpen = false;
            
            if (hintsActive)
            {
                hintsActive = false;
                ToggleHintArrows(false);
            }
        }
    }
}
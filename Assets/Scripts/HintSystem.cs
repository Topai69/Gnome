using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HintSystem : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject hintPromptPanel;
    [SerializeField] private TextMeshProUGUI hintPromptText;
    [SerializeField] private GameObject taskSelectionPanel;
    [SerializeField] private Button[] taskSelectionButtons;
    [SerializeField] private string[] taskNames;
    [SerializeField] private GameObject[] taskArrowGroups;
    [SerializeField] private GameObject blurPanel;
    [SerializeField] private GameObject gameplayUIElements;
    
    [Header("External References")]
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private PlayerMovement playerMovement;
    
    private bool hintsActive = false;
    private int currentTaskIndex = -1;
    private bool isTaskSelectionOpen = false;
    private GameObject lastSelectedButton = null;
    private bool wasHintPromptVisibleBeforePause = false;
    
    void Start()
    {
        HideAllArrows();
        SetupTaskButtons();
        
        if (hintPromptPanel != null)
            hintPromptPanel.SetActive(true);
            
        if (taskSelectionPanel != null)
            taskSelectionPanel.SetActive(false);
            
        if (blurPanel != null)
            blurPanel.SetActive(false);
            
        if (pauseMenu == null)
            pauseMenu = FindObjectOfType<PauseMenu>();
            
        if (playerMovement == null)
            playerMovement = FindObjectOfType<PlayerMovement>();
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
        if (pauseMenu != null && pauseMenu.isPaused)
            return;
            
        if (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            ToggleTaskSelection();
        }
        
        if (isTaskSelectionOpen && EventSystem.current.currentSelectedGameObject != lastSelectedButton)
        {
            if (lastSelectedButton != null)
            {
                TaskButtonHighlight lastIcon = lastSelectedButton.GetComponent<TaskButtonHighlight>();
                if (lastIcon != null)
                    lastIcon.OnDeselect(new BaseEventData(EventSystem.current));
            }
            
            lastSelectedButton = EventSystem.current.currentSelectedGameObject;
            
            if (lastSelectedButton != null)
            {
                TaskButtonHighlight currentIcon = lastSelectedButton.GetComponent<TaskButtonHighlight>();
                if (currentIcon != null)
                    currentIcon.OnSelect(new BaseEventData(EventSystem.current));
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
            
            if (blurPanel != null)
                blurPanel.SetActive(false);
                
            if (gameplayUIElements != null)
                gameplayUIElements.SetActive(true);
                
            EventSystem.current.SetSelectedGameObject(null);
            
            UnblockPlayerMovement();
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
            
        ClearHints();
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
            
            if (blurPanel != null)
                blurPanel.SetActive(false);
                
            if (gameplayUIElements != null)
                gameplayUIElements.SetActive(true);
                
            UnblockPlayerMovement();
        }
        else
        {
            taskSelectionPanel.SetActive(true);
            isTaskSelectionOpen = true;
            
            if (taskSelectionButtons != null && taskSelectionButtons.Length > 0 && taskSelectionButtons[0] != null)
            {
                EventSystem.current.SetSelectedGameObject(taskSelectionButtons[0].gameObject);
            }
            
            if (blurPanel != null)
                blurPanel.SetActive(true);
                
            if (gameplayUIElements != null)
                gameplayUIElements.SetActive(false);
                
            BlockPlayerMovement();
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
    
    public void OnGamePaused()
    {
        if (hintPromptPanel != null)
            wasHintPromptVisibleBeforePause = hintPromptPanel.activeSelf;
        
        if (taskSelectionPanel != null)
            taskSelectionPanel.SetActive(false);
    
        if (hintPromptPanel != null)
            hintPromptPanel.SetActive(false);
        
        if (blurPanel != null)
            blurPanel.SetActive(false);
        
        isTaskSelectionOpen = false;
        hintsActive = false;
        HideAllArrows();
    }
    
    public void OnGameResumed()
    {
        UnblockPlayerMovement();
        
        if (hintPromptPanel != null)
            hintPromptPanel.SetActive(wasHintPromptVisibleBeforePause);
    }
    
    public void OnTaskCompleted(int taskIndex)
    {
        if (taskIndex == currentTaskIndex)
        {
            hintsActive = false;
            ToggleHintArrows(false);
        }
    }
    
    private void BlockPlayerMovement()
    {
        if (playerMovement != null)
        {
            playerMovement.blockAInput = true;
            playerMovement.blockJump = true;
            playerMovement.enabled = false;
        }
    }
    
    private void UnblockPlayerMovement()
    {
        if (playerMovement != null)
        {
            playerMovement.blockAInput = false;
            playerMovement.blockJump = false;
            playerMovement.enabled = true;
        }
    }
    
    public void CloseTaskSelection()
    {
        if (isTaskSelectionOpen)
        {
            taskSelectionPanel.SetActive(false);
            isTaskSelectionOpen = false;
            
            if (blurPanel != null)
                blurPanel.SetActive(false);
                
            if (gameplayUIElements != null)
                gameplayUIElements.SetActive(true);
                
            if (hintsActive)
            {
                hintsActive = false;
                ToggleHintArrows(false);
            }
            
            UnblockPlayerMovement();
        }
    }
    
    private void ClearHints()
    {
        hintsActive = false;
        currentTaskIndex = -1;
        HideAllArrows();
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskManager : MonoBehaviour
{
    [System.Serializable]
    public class TaskBinding
    {
        public TaskItem taskItem;
        public string description;
        public bool isCompleted;
    }

    public List<TaskBinding> taskBindings;
    [SerializeField] private Image progressBar; 
    [SerializeField] private TextMeshProUGUI percentageText;
    [SerializeField] private Image pauseMenuProgressBar;
    [SerializeField] private TextMeshProUGUI pauseMenuPercentageText;
    [SerializeField] private GameObject endingBox;

    void Start()
    {
        foreach (var binding in taskBindings)
        {
            binding.taskItem.SetTask(binding.description, binding.isCompleted);
        }
        
        UpdateTaskProgress();
    }

    // ✅ Call this to mark a task as completed and update the UI
    public void CompleteTask(int index)
    {
        if (index >= 0 && index < taskBindings.Count)
        {
            taskBindings[index].isCompleted = true;
            taskBindings[index].taskItem.SetTask(taskBindings[index].description, true);
            UpdateTaskProgress();
            
            HintSystem hintSystem = FindObjectOfType<HintSystem>();
            if (hintSystem != null)
            {
                hintSystem.OnTaskCompleted(index);
            }

           
        }
        else
        {
            Debug.LogWarning("TaskManager: Tried to complete an invalid task index.");
        }
    }
    
    private void UpdateTaskProgress()
    {
        int completedCount = 0;
        foreach (TaskBinding binding in taskBindings)
        {
            if (binding.isCompleted)
                completedCount++;
        }

        float progress = completedCount * (1.0f / 6.0f);
        
        
        if (progressBar != null)
            progressBar.fillAmount = progress;

        if (pauseMenuProgressBar != null)
            pauseMenuProgressBar.fillAmount = progress;

        int percentage = Mathf.RoundToInt(progress * 100);
        
        if (percentageText != null)
            percentageText.text = percentage + "%";
            
        if (pauseMenuPercentageText != null)
            pauseMenuPercentageText.text = percentage + "%";
        if (percentage >= 100)
        {
            endingBox.GetComponent<EndingScript>().Ending();
        }
    }

    public float GetCurrentProgress()
    {
        int completedCount = 0;
        foreach (TaskBinding binding in taskBindings)
        {
            if (binding.isCompleted)
                completedCount++;
        }
        
        return taskBindings.Count > 0 ? (float)completedCount / taskBindings.Count : 0f;
    }
}

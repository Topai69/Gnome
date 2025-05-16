using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        foreach (TaskBinding binding in taskBindings)
        {
            if (binding.taskItem != null)
            {
                binding.taskItem.SetTask(binding.description, binding.isCompleted);
            }
        }
    }

    // ✅ Call this to mark a task as completed and update the UI
    public void CompleteTask(int index)
    {
        if (index >= 0 && index < taskBindings.Count)
        {
            taskBindings[index].isCompleted = true;
            taskBindings[index].taskItem.SetTask(taskBindings[index].description, true);
        }
        else
        {
            Debug.LogWarning("TaskManager: Tried to complete an invalid task index.");
        }
    }
}

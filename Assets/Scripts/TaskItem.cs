using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour
{
    public TextMeshProUGUI taskText;
    public Toggle taskToggle;

    public void SetTask(string task, bool isCompleted)
    {
        taskText.text = task;
        taskToggle.isOn = isCompleted;
    }
}

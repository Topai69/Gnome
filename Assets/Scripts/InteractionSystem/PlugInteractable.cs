using UnityEngine;

public class PlugInteractable : InteractableBase
{

    [SerializeField] private TaskManager taskManager;
    [HideInInspector] public ScoreScript ScoreScript;

    private void Start()
    {
        ScoreScript = FindAnyObjectByType<ScoreScript>();
    }
    public override void OnInteract()
    {
        base.OnInteract();
        
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().mass = 5.0f;

        if (taskManager != null)
        {
            taskManager.CompleteTask(3);
        }

        if (ScoreScript != null)
        {
            ScoreScript.Score += 10;
        }
        gameObject.GetComponent<PlugInteractable>().enabled = false; //turn of the script, since it's no longer needed
        gameObject.layer = 6;
    }
}
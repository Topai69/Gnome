using UnityEngine;

public class RightSinkValveInteractable : InteractableBase
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
        Debug.Log("Interacted with left sink valve");
        (transform.parent.gameObject.GetComponent<Animation>()).Play("RightSinkValve");
        
        if (taskManager != null)
        {
            taskManager.CompleteTask(1);
        }

        if (ScoreScript != null)
        {
            ScoreScript.Score += 10;
        }
        gameObject.GetComponent<RightSinkValveInteractable>().enabled = false; //turn of the script, since it's no longer needed
        gameObject.layer = 6;
    }
}
using UnityEngine;

public class LeftSinkValveInteractable : InteractableBase
{

    private void Start()
    {
        ScoreScript = FindAnyObjectByType<ScoreScript>();
    }
    [SerializeField] private TaskManager taskManager;
    [HideInInspector] public ScoreScript ScoreScript;


    public override void OnInteract()
    {
        base.OnInteract();
        Debug.Log("Interacted with left sink valve");
        //anim.Play("Rotating");
        (transform.parent.gameObject.GetComponent<Animation>()).Play("LeftSinkValve");

        if (taskManager != null)
        {
            taskManager.CompleteTask(1);
        }

        if (ScoreScript != null)
        {
            ScoreScript.Score += 10;
        }

        gameObject.GetComponent<LeftSinkValveInteractable>().enabled = false; //turn of the script, since it's no longer needed
        gameObject.layer = 6;
    }
}
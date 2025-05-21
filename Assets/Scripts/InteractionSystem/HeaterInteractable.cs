using UnityEngine;

public class HeaterInteractable : InteractableBase
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
        Debug.Log("Interacted with heater");
        //anim.Play("Rotating");
        transform.parent.gameObject.GetComponent<Animation>().Play("Activation");

        if (taskManager != null)
        {
            taskManager.CompleteTask(1); 
        }

        if (ScoreScript != null)
        {
            ScoreScript.Score += 20;
        }

        gameObject.GetComponent<HeaterInteractable>().enabled = false; //turn of the script, since it's no longer needed
        gameObject.layer = 6;
    }
}

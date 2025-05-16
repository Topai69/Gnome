using UnityEngine;

public class HeaterInteractable : InteractableBase
{
    [SerializeField] private AnimationClip rotation;
    [SerializeField] private TaskManager taskManager;
    [HideInInspector] public ScoreScript ScoreScript; 

    private Animation anim;

    /* private void Start()
    {
        anim = transform.parent.GetComponent<Animation>();
        anim.AddClip(rotation, "Rotating");
    }*/

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
    }
}

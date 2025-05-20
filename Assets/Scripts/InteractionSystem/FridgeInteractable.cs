using UnityEngine;

public class FridgeInteractable : InteractableBase
{
    [SerializeField] private AnimationClip rotation;
    [SerializeField] private TaskManager taskManager;
    [HideInInspector] public ScoreScript ScoreScript; 

    /*private void Start()
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
        Debug.Log("Interacted with fridge");
        //anim.Play("Rotating");
        ((transform.parent).parent.gameObject.GetComponent<Animation>()).Play("Fridge");

        if (taskManager != null)
        {
            taskManager.CompleteTask(0); 
        }

        if (ScoreScript != null)
        {
            ScoreScript.Score += 20;
        }
    }
}
using UnityEngine;

public class HeaterInteractable : InteractableBase
{
    [SerializeField] private AnimationClip rotation;
    [SerializeField] private TaskManager taskManager;
    [HideInInspector] public ScoreScript ScoreScript;

    [SerializeField] private GameObject quickTimeEvent;
    [SerializeField] private GameObject interactionUI; 

    [HideInInspector] public bool hasInteracted = false;

    private void Start()
    {
        ScoreScript = FindAnyObjectByType<ScoreScript>();
    }

    public override void OnInteract()
    {
        if (hasInteracted) return;

        base.OnInteract();
        Debug.Log("Interacted with heater — QTE triggered");

        if (quickTimeEvent != null && !quickTimeEvent.activeSelf)
        {
            quickTimeEvent.SetActive(true);
        }
        FinishHeaterInteraction();
    }

    public void FinishHeaterInteraction()
    {
        Debug.Log("Heater QTE succeeded — playing animation");

        transform.parent.gameObject.GetComponent<Animation>().Play("Activation");

        if (taskManager != null)
        {
            taskManager.CompleteTask(1);
        }

        if (ScoreScript != null)
        {
            ScoreScript.Score += 20;
        }

        hasInteracted = true;

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        GetComponent<HeaterInteractable>().enabled = false;
        gameObject.layer = 6;
    }
}

using GogoGaga.OptimizedRopesAndCables;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class PlugInteractable_2 : InteractableBase
{

    [SerializeField] private TaskManager taskManager;
    [HideInInspector] public ScoreScript ScoreScript;
    private bool flag = false;
    private float timer = 1f;
    private float timePassed = 0f;
    [SerializeField] Rope Rope;
    [SerializeField] Transform center;
    [SerializeField] float length;

    private void Start()
    {
        ScoreScript = FindAnyObjectByType<ScoreScript>();
    }
    public override void OnInteract()
    {
        base.OnInteract();

        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().mass = 25.0f;
        gameObject.GetComponent<Rigidbody>().angularDamping = 1f;
        gameObject.GetComponent<Rigidbody>().centerOfMass = center.localPosition;

        if (taskManager != null)
        {
            taskManager.CompleteTask(4);
        }

        if (ScoreScript != null)
        {
            ScoreScript.Score += 10;
        }
        flag = true;
        gameObject.layer = 6;
        
        
    }

    private void Update()
    {
        if (flag)
        {
            if (timePassed < timer)
            {

                timePassed += Time.deltaTime;
            }
            else
            {
                Debug.Log("Removed");
                Rope.ropeLength = length;
                Destroy(GetComponent<Rigidbody>());
                gameObject.GetComponent<PlugInteractable>().enabled = false;
            }
        }
    }

}
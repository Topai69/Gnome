
using UnityEngine;


public class PlugScript : InteractableBase
{
    public float t;
    public Vector3 startPosition;
    public Vector3 target;
    public float timeToReachTarget;
    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        
    }

    public override void OnInteract()
    {
        base.OnInteract();
        t += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, target, t);
    }
    public void SetDestination(Vector3 destination, float time)
    {
        t = 0;
        startPosition = transform.position;
        timeToReachTarget = time;
        target = destination;
    }
}

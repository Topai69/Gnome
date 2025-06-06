using UnityEngine;

public class ArrowBobAnimation : MonoBehaviour
{
    [SerializeField] private float bobSpeed = 1.0f;
    [SerializeField] private float bobAmount = 0.2f;
    
    private Vector3 startPos;
    
    void Start()
    {
        startPos = transform.position;
    }
    
    void Update()
    {
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * bobSpeed) * bobAmount;
    }
}
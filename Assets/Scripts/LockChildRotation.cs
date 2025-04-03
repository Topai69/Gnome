using UnityEngine;

public class LockChildRotation : MonoBehaviour
{
    private Quaternion initialLocalRotation;

    void Start()
    {
        initialLocalRotation = transform.localRotation;
    }

    void LateUpdate()
    {
        transform.localRotation = initialLocalRotation; // Keep child from tilting
    }
}

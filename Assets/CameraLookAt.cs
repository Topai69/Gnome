using UnityEngine;

[ExecuteAlways]
public class CameraLookAt : MonoBehaviour {
    bool flip;

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying)
        {
            transform.LookAt(Camera.main.transform.position, Vector3.up);
        }
    }
}

using UnityEngine;

public class WaterDripping : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] public AudioClip voiceLine;
    void Start()
    {
        GetComponent<AudioSource>().loop = true;
    }

    // Update is called once per frame
    void Stop()
    {
        GetComponent<AudioSource>().Stop();
    }
}

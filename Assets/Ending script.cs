using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class EndingScript : MonoBehaviour
{
    public GameObject DialogueBox;
    public AudioSource audioSource;
    public AudioClip voiceLine;
    [SerializeField] public Scene scene;

    private void Start()
    {
        this.gameObject.SetActive(true);
    }
    
    public void Ending()
    {
        audioSource.PlayOneShot(voiceLine);
        DialogueBox.SetActive(true);
        GetComponent<BoxCollider>().enabled = false;
        SceneManager.LoadScene(3);
    }
}

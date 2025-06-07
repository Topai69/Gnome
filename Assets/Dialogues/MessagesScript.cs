using UnityEngine;
using System.Collections.Generic;

public class MessagesScript : MonoBehaviour
{
    public GameObject DialogueBox;
    public AudioSource audioSource;
    public AudioClip voiceLine;

    private void Start()
    {
        this.gameObject.SetActive(true);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !Dialogue.isAnyDialogueActive)
        {
            audioSource.PlayOneShot(voiceLine);
            DialogueBox.SetActive(true);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}

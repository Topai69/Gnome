using UnityEngine;
using System.Collections.Generic;

public class MessagesScript : MonoBehaviour
{
    public GameObject messageUI;
    private List<string> messages;
    private int currentIndex;
    public bool isShowing;

    public List<string> predefinedMessages; // assign in Inspector

    public void StartMessages(List<string> newMessages)
    {
        messages = newMessages;
        currentIndex = 0;
        isShowing = true;
        messageUI.SetActive(true);
        ShowCurrentMessage();
        Time.timeScale = 0;
    }

    void ShowCurrentMessage()
    {
        // Your logic for showing the message
        Debug.Log(messages[currentIndex]);
    }

    private void OnTriggerEnter(Collider other) // Use OnTriggerEnter2D if 2D
    {
        if (other.CompareTag("Player"))
        {
            StartMessages(predefinedMessages);
        }
    }

    public void EndDialogue()
    { 
        Time.timeScale = 1;
        messageUI.SetActive(false);
    }
}

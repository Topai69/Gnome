using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public int voiceDuration;

    private int index;
    
    public static bool isAnyDialogueActive = false;

    // Start is called before the first frame update
    protected void Start()
    {
        if (isAnyDialogueActive)
        {
            gameObject.SetActive(false);
            return;
        }
        
        isAnyDialogueActive = true;
        textComponent.text = string.Empty;

        if (BackgroundMusicManager.Instance != null)
            BackgroundMusicManager.Instance.FadeToLow();

        StartDialogue();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        
        if (index == lines.Length - 1)
        {
            yield return new WaitForSeconds(voiceDuration);
            DisableDialogue();
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            DisableDialogue();
        }
    }
    
    void DisableDialogue()
    {
        isAnyDialogueActive = false;

        if (BackgroundMusicManager.Instance != null)
                BackgroundMusicManager.Instance.FadeToNormal();

        gameObject.SetActive(false);
    }
    
    void OnDisable()
    {
        if (gameObject.activeInHierarchy == false)
        {
            isAnyDialogueActive = false;

            if (BackgroundMusicManager.Instance != null)
                BackgroundMusicManager.Instance.FadeToNormal();
        }
    }
}


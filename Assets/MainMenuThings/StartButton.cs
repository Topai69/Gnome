using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Video;
    public GameObject Image;
    public GameObject VideoPlayer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        StartCoroutine(ChangeScene());
        Image.SetActive(false);
        Video.SetActive(true);
        VideoPlayer.SetActive(true);
    }
    IEnumerator ChangeScene()
    { 
        yield return new WaitForSeconds(3f); // 3 seconds delay
        SceneManager.LoadScene(2);
        VideoPlayer.SetActive(false);
    }   
    
}

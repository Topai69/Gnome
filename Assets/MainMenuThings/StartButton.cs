using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public GameObject Video;
    public GameObject Image;
    public GameObject VideoPlayer;
    public GameObject LoadingScreen; 
    public float delayBeforeLoad = 5f; // Delay Lenght

    public void StartGame()
    {
        StartCoroutine(ChangeScene());
        Image.SetActive(false);
        Video.SetActive(true);
        VideoPlayer.SetActive(true);
        if (LoadingScreen != null) LoadingScreen.SetActive(true);
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(delayBeforeLoad);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

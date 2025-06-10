using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StartButton : MonoBehaviour
{
    public GameObject Video;
    public GameObject Image;
    public GameObject VideoPlayer;
    public GameObject LoadingScreen;

    public float delayBeforeLoad = 5f; // Delay Lenght

    public void StartGame()
    {
        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence()
    {
        if (Image != null) Image.SetActive(false);
        if (Video != null) Video.SetActive(true);
        if (VideoPlayer != null) VideoPlayer.SetActive(true);

        VideoPlayer vp = VideoPlayer.GetComponent<VideoPlayer>();
        if (vp != null)
        {
            vp.Play();

            yield return new WaitUntil(() => vp.isPrepared);
            yield return new WaitUntil(() => !vp.isPlaying);

            VideoPlayer.SetActive(false);
            if (Video != null) Video.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(2f); 
        }

        if (LoadingScreen != null)
            LoadingScreen.SetActive(true);

        yield return new WaitForSeconds(delayBeforeLoad);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
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

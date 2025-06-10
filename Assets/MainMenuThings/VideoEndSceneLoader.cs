using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class VideoEndSceneLoader : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject loadingScreen; // Assign a UI panel or image in the Inspector

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        videoPlayer.loopPointReached += OnVideoFinished;

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(LoadSceneWithLoadingScreen(2));
    }

    IEnumerator LoadSceneWithLoadingScreen(int sceneIndex)
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(sceneIndex);
    }
}

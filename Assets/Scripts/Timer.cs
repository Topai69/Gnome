using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float timer = 20.0f;
    public TextMeshProUGUI timerText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(TimerDown());
        timerText.text = timer.ToString();

        if (timer >= 0)
        {
                Application.Quit();
        }
    }

    IEnumerator TimerDown()
    {
        timer -= 1 * Time.deltaTime;
        yield return new WaitForSeconds(1f);
    }
}

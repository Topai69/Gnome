using UnityEngine;
using UnityEngine.UIElements;

public class ScoreScript : MonoBehaviour
{
    public int Score;
    public ProgressBar ProgressBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int ScoreIncrease = 10;
    public int level;
    void Start()
    {
        ProgressBar = FindAnyObjectByType<ProgressBar>();
        level = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ProgressBar.current = Score;
        Leveling();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Score")
        {
                Score += ScoreIncrease;
        }
    }

    void Leveling()
    {
        if (level == 0 && Score >= 100)
        {
            level = 1;
            Score = 0;
            ProgressBar.maximum = 300;
        }
        if (level == 1 && Score >= 300)
        {
            level = 2;
            Score = 0;
            ProgressBar.maximum = 1000;
        }
    }

}

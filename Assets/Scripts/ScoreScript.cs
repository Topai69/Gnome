using UnityEngine;
using UnityEngine.UIElements;

public class ScoreScript : MonoBehaviour
{
    public int Score;
    public ProgressBar ProgressBar;
    public int ScoreIncrease = 10;
    public int level;
    void Start()
    {
        ProgressBar = FindAnyObjectByType<ProgressBar>();
        level = 1;
    }

    // Update is called once per frame
    void Update()
    {
        ProgressBar.current = Score;
        Leveling();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Score")
        {
            Score += ScoreIncrease;
        }
    }

    void Leveling ()
    {
        if (level == 1 && Score >= 100)
        {
            level = 2;
            Score = 0;
            ProgressBar.maximum = 300;
            
        }
        if (level == 2 && Score >= 300)
        {
            level = 3;
            Score = 0;
            ProgressBar.maximum = 500;
            
        }
    }
}
using UnityEngine;

public class RotateOverTime : PauseMenu
{
    public float duration = 10f; // in seconds
    private float rotationAmount = 360f;
    public float elapsedTime;
    public float gameDuration;


    void Update()
    {
        elapsedTime += Time.deltaTime;
        float rotationSpeed = rotationAmount / duration; 
        transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);


        if (elapsedTime > gameDuration)
        {
            //Application.Quit(); //
            Time.timeScale = 0f;
        }
    }
}
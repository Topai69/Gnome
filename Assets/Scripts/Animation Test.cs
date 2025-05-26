using UnityEngine;
using UnityEngine.Windows;

public class AnimationTest : MonoBehaviour
{
    private Animator Gnome;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Gnome = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.T))
        {
            Gnome.SetBool("IsIdle",true);
            Debug.Log("T Pressed");
        }


    //    if (animator.SetTrigger((Input.GetKeyDown("t")));
    //    {
    //        animator.Play("Gnome");
    //    }
    }
}

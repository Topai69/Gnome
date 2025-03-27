using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;
public class Switch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    public GameObject Player_obj1;

    [SerializeField]
    public GameObject Player_obj2;

    int i = 1;
    Renderer rend1;
    Renderer rend2;

    private void Start()
    {
        rend2 = Player_obj2.GetComponent<Renderer>();
        rend2.enabled = false;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (i == 1)
            {
                rend1 = Player_obj1.GetComponent<Renderer>();
                rend1.enabled = false;
                rend2 = Player_obj2.GetComponent<Renderer>();
                rend2.enabled = true;
                i = 2;
            }
            else if (i == 2)
            {
                rend2 = Player_obj2.GetComponent<Renderer>();
                rend2.enabled = false;
                rend1 = Player_obj1.GetComponent<Renderer>();
                rend1.enabled = true;
                i = 1;
            }    
        }
    }
}

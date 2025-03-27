using UnityEngine;

public class Interaction : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject Plug;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UnPlug();
        }
    }
    void UnPlug()
    {
        Plug.transform.position = new Vector3(-30.2130966f, 0.381148338f, 18.9699993f);
        
    }


}

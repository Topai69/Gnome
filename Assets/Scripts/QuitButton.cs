using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuitButton : MonoBehaviour
{
    private Button quitButton;
    
    void Start()
    {
        quitButton = GetComponent<Button>();
        quitButton.onClick.AddListener(ExitApplication);
    }
    
    public void ExitApplication()
    {
        Debug.Log("Quit button clicked - Exiting application");
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
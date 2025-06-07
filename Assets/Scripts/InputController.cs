using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    [SerializeField] private float mouseDetectionThreshold = 0.5f;
    
    private Vector3 lastMousePosition;
    private bool usingMouse = true;
    
    void Start()
    {
        lastMousePosition = Input.mousePosition;
    }
    
    void Update()
    {
        bool mouseMovement = Vector3.Distance(lastMousePosition, Input.mousePosition) > mouseDetectionThreshold;
        bool mouseClicked = Input.GetMouseButtonDown(0);
        lastMousePosition = Input.mousePosition;

        bool controllerInput = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f || 
                             Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f ||
                             Input.GetButtonDown("Submit") || 
                             Input.GetButtonDown("Cancel");

        if (mouseMovement || mouseClicked)
        {
            if (!usingMouse)
            {
                SwitchToMouse();
            }
        }
        else if (controllerInput && usingMouse)
        {
            SwitchToController();
        }
    }
    
    private void SwitchToMouse()
    {
        usingMouse = true;
        EventSystem.current.SetSelectedGameObject(null);
    }
    
    private void SwitchToController()
    {
        usingMouse = false;

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            Selectable[] selectables = FindObjectsOfType<Selectable>();
            if (selectables.Length > 0)
            {
                foreach (Selectable s in selectables)
                {
                    if (s.gameObject.activeInHierarchy && s.interactable)
                    {
                        EventSystem.current.SetSelectedGameObject(s.gameObject);
                        break;
                    }
                }
            }
        }
    }
}
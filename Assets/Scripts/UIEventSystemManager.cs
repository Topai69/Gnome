using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class UIEventSystemManager : MonoBehaviour
{
    private EventSystem eventSystem;
    
    void Start()
    {
        eventSystem = GetComponent<EventSystem>();
        if (eventSystem == null)
            eventSystem = FindObjectOfType<EventSystem>();
    }
    
    void Update()
    {
        bool mouseMovement = Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0;
        bool mouseClick = Input.mousePresent && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1));
        
        if (mouseMovement || mouseClick)
        {
            eventSystem.SetSelectedGameObject(null);
        }
        if (eventSystem.currentSelectedGameObject == null && 
            (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {

            GameObject firstButton = FindFirstSelectableUI();
            if (firstButton != null)
                eventSystem.SetSelectedGameObject(firstButton);
        }
    }
    
    private GameObject FindFirstSelectableUI()
    {
        Selectable[] selectables = FindObjectsOfType<Selectable>();
        if (selectables.Length > 0)
        {
            foreach (Selectable s in selectables)
            {
                if (s.gameObject.activeInHierarchy && s.interactable)
                    return s.gameObject;
            }
        }
        return null;
    }
}
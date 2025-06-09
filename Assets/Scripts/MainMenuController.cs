using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button defaultButton;
    private bool usingController = false;

    void Start()
    {
        if (defaultButton != null)
        {
            EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
        }
    }

    void Update()
    {
        bool mouseMovement = Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f;
        bool mouseClick = Input.GetMouseButtonDown(0);

        bool controllerInput = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f ||
                               Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f ||
                               Input.GetButtonDown("Submit") ||
                               Input.GetButtonDown("Cancel");

        if (mouseMovement || mouseClick)
        {
            if (usingController)
            {
                usingController = false;
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
        else if (controllerInput)
        {
            usingController = true;
            if (EventSystem.current.currentSelectedGameObject == null && defaultButton != null)
            {
                EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
            }
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class KeypadButton : MonoBehaviour
{
    [SerializeField] private string buttonValue;
    private Button button;
    private KeypadInteraction keypad;

    private void Start()
    {
        button = GetComponent<Button>();
        keypad = FindObjectOfType<KeypadInteraction>();
        
        if (button != null && keypad != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    private void OnButtonClick()
    {
        if (keypad != null)
        {
            keypad.AddNumber(buttonValue);
        }
    }
} 
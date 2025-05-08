using UnityEngine;
using UnityEngine.UI;

public class KeypadSpecialButton : MonoBehaviour{
    public enum ButtonType{
        Enter,
        Clear
    }

    [SerializeField] private ButtonType buttonType;
    private Button button;
    private KeypadInteraction keypad;

    private void Start(){
        button = GetComponent<Button>();
        keypad = FindObjectOfType<KeypadInteraction>();

        if (button != null && keypad != null){
            button.onClick.AddListener(OnButtonClick);
        }
    }

    private void OnButtonClick(){
        if (keypad != null){
            switch (buttonType){
                case ButtonType.Enter:
                    keypad.SubmitCode();
                    break;
                case ButtonType.Clear:
                    keypad.ClearInput();
                    break;
            }
        }
    }
}
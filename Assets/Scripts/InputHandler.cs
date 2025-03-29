using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Header ("Input Data")]
    //public CameraInputData cameraInputData;
    //public MovementInputData movementInputData;
    public InteractionInputData interactionInputData;


    void Start(){

        //cameraInputData.ResetInput();
        //movementInputData.ResetInput();
        interactionInputData.Reset();
    }

    void Update(){

        //GetCameraInput();
        //GetMovementInputData();
        GetInteractionInputData();
    }

    void GetInteractionInputData(){

        interactionInputData.InteractClicked = Input.GetKeyDown(KeyCode.E);
        interactionInputData.InteractRelease = Input.GetKeyUp(KeyCode.E);
    }
 
}
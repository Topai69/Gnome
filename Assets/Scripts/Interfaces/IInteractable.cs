using UnityEngine;

public interface IInteractable{
    
    float HoldDuration { get; }
    bool HoldInteract { get; }
    bool MultipleUse {get; }
    bool isInteractable {get;}

    void OnInteract();
}
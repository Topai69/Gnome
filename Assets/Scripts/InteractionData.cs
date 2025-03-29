using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Interaction Data", menuName = "InteractionSystem/InteractionData")]

public class InteractionData : ScriptableObject {
    
    private InteractableBase m_interactable;

    public InteractableBase Interactable{

        get => m_interactable;
        set => m_interactable = value;
    }

    //this method calls the other on interact in InteractableBase
    public void Interact(){

        m_interactable.OnInteract();
        ResetData();
    }

    public bool IsSameInteractable(InteractableBase _newInteractable) => m_interactable == _newInteractable;

    //remember to check (if i forget to delete it, its a reminder for me)
    public void ResetData() => m_interactable = null;

    public bool IsEmpty() => m_interactable == null;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour{

    [Header("Data")]
    public InteractionInputData interactionInputData;
    public InteractionData interactionData;

    [Space]
    [Header("Ray Settings")]
    public float rayDistance;
    public float raySphereRadius;
    public LayerMask interactableLayer;

    private Camera m_cam;

    void Awake()
    {
        m_cam = FindObjectOfType<Camera>();
    }

    void Update()
    {
        CheckForInteractable();
        CheckForInteractableInput();
    }

    //maybe a bit messy but a fast summary -> if we hit something we check if there's a interactablebase component
    //if it does, we check if interactiondata is empty (if yes, we assign it the new interactable), else we check if its the same object
    void CheckForInteractable()
    {
        Ray _ray = new Ray(m_cam.transform.position, m_cam.transform.forward);
        RaycastHit _hitInfo;

        bool _hitSomething = Physics.SphereCast (_ray, raySphereRadius, out _hitInfo, rayDistance, interactableLayer);

        if (_hitSomething){
            InteractableBase _interactable = _hitInfo.transform.GetComponent<InteractableBase>();

            if(interactionData.IsEmpty()){
                interactionData.Interactable = _interactable;
            }else{
                if (!interactionData.IsSameInteractable(_interactable)){
                    interactionData.Interactable = _interactable;
                }
            }

        }else{
            interactionData.ResetData();
        }

        Debug.DrawRay(_ray.origin, _ray.direction * rayDistance, _hitSomething ? Color.green : Color.red);
    }

    void CheckForInteractableInput()
    {

    }    
    
}
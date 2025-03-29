using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour{

    [Header("Data")]
    [SerializeField] private InteractionInputData interactionInputData = null;
    [SerializeField] private InteractionData interactionData = null;

    [Space, Header("UI")]
    [SerializeField] private InteractionUIPanel uiPanel;

    [Space]
    [Header("Ray Settings")]
    [SerializeField] private float rayDistance = 0f;
    [SerializeField] private float raySphereRadius = 0f;
    [SerializeField] private LayerMask interactableLayer = ~0;

    private Camera m_cam;
    private bool m_interacting;
    private float m_holderTimer = 0f;

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
                uiPanel.SetTooltip("Interact");
            }else{
                if (!interactionData.IsSameInteractable(_interactable)){
                    interactionData.Interactable = _interactable;
                    uiPanel.SetTooltip("Interact");
                }
            }

        }else{
            uiPanel.ResetUI();
            interactionData.ResetData();
        }

        Debug.DrawRay(_ray.origin, _ray.direction * rayDistance, _hitSomething ? Color.green : Color.red);
    }

    void CheckForInteractableInput()
    {
        if(interactionData.IsEmpty())
            return;

        if(interactionInputData.InteractClicked){
            m_interacting = true;
            m_holderTimer = 0f;
        }

        if (interactionInputData.InteractRelease){
            m_interacting = false;
            m_holderTimer = 0f;
            uiPanel.UpdateProgressBar(0f);
        }

        if (m_interacting){
            if(!interactionData.Interactable.IsInteractable)
                return;
            
            if (interactionData.Interactable.HoldInteract){
                m_holderTimer += Time.deltaTime;

                float heldPercentage = m_holderTimer / interactionData.Interactable.HoldDuration;
                uiPanel.UpdateProgressBar(heldPercentage);

                if (heldPercentage > 1f){
                    interactionData.Interact();
                    m_interacting = false;
                }
            }else{
                interactionData.Interact();
                m_interacting = false;
            }
        }
    }    
    
}
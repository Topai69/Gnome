using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInteractable : InteractableBase{

    public override void OnInteract(){
        Debug.Log("start");
        base.OnInteract();
        Destroy(gameObject);
    }
}
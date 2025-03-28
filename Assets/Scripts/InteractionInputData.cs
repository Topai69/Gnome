using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractionInputData", menuName = "InteractionSystem/InputData")]

public class InteractionInputData : ScriptableObject
{
    private bool m_interactedClicked;
    private bool m_interactedRelease;

    public bool InteractClicked{
        get => m_interactedClicked;
        set => m_interactedClicked = value;
    }

    public bool InteractRelease{
        get => m_interactedRelease;
        set => m_interactedRelease = value;
    }

    //scriptable object remembers the value after finishing the game, need a reset method
    public void Reset(){
        m_interactedClicked = false;
        m_interactedRelease = false;
    }
}
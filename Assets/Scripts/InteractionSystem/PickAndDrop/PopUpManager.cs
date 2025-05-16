using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] GameObject PopUp_prefab;
    
    public GameObject GetPopUpPrefab()
    {
        return PopUp_prefab;
    }

    void Update()
    {
        
    }
}
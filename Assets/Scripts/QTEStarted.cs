using System.Collections;
using UnityEngine;

public class QTEStarter : MonoBehaviour
{
    public GameObject qteObject; 

    void Start()
    {
        StartCoroutine(ActivateQTEAfterDelay());
    }

    IEnumerator ActivateQTEAfterDelay()
    {
        yield return new WaitForSeconds(3f);

        qteObject.SetActive(true); 
    }
}

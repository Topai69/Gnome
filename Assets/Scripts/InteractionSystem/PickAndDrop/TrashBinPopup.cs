using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TrashBinPopup : MonoBehaviour
{
    [SerializeField] Text text;
    public string text_value;

    void Start()
    {
        text.text = text_value;
        Destroy(gameObject, 2.5f);
    }
}
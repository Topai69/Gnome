using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TaskButtonHighlight : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private GameObject selectionIndicator;
    [SerializeField] private float scaleMultiplier = 1.1f;
    [SerializeField] private Color highlightColor = new Color(1f, 1f, 0.5f);
    
    private Image buttonImage;
    private Color normalColor;
    
    void Start()
    {
        buttonImage = GetComponent<Image>();
        if (buttonImage != null)
            normalColor = buttonImage.color;
       
        if (selectionIndicator != null)
            selectionIndicator.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (selectionIndicator != null)
            selectionIndicator.SetActive(true);

        transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, 1f);
        
        if (buttonImage != null)
            buttonImage.color = highlightColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (selectionIndicator != null)
            selectionIndicator.SetActive(false);
        transform.localScale = Vector3.one;

        if (buttonImage != null)
            buttonImage.color = normalColor;
    }
}
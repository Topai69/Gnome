using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHighlight : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private float scaleMultiplier = 1.1f;
    [SerializeField] private Color highlightColor = new Color(1f, 0.9f, 0.5f);
    
    private Image buttonImage;
    private Color normalColor;
    
    void Start()
    {
        buttonImage = GetComponent<Image>();
        if (buttonImage != null)
            normalColor = buttonImage.color;
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, 1f);
        
        if (buttonImage != null)
            buttonImage.color = highlightColor;
    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        transform.localScale = Vector3.one;
        
        if (buttonImage != null)
            buttonImage.color = normalColor;
    }
}
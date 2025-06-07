using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PauseButtonHighlight : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Highlight Settings")]
    [SerializeField] private float scaleMultiplier = 1.1f;
    [SerializeField] private Color highlightColor = new Color(1f, 0.9f, 0.5f); 
    [SerializeField] private float glowIntensity = 0.5f;
    
    [Header("Components")]
    [SerializeField] private Image outlineImage; 
    
    private Image buttonImage;
    private TextMeshProUGUI buttonText;
    private Color normalColor;
    private Color normalTextColor;
    private Vector3 originalScale;
    private bool isSelected = false;
    private bool isHovered = false;
    
    void Awake()
    {
        buttonImage = GetComponent<Image>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        originalScale = transform.localScale;
        
        if (buttonImage != null)
            normalColor = buttonImage.color;
            
        if (buttonText != null)
            normalTextColor = buttonText.color;
            
        if (outlineImage != null)
            outlineImage.enabled = false;
    }
   
    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
        UpdateVisuals();
    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        UpdateVisuals();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        UpdateVisuals();
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        bool shouldHighlight = isSelected || isHovered;
        
        transform.localScale = shouldHighlight 
            ? originalScale * scaleMultiplier 
            : originalScale;
        
        if (buttonImage != null)
        {
            buttonImage.color = shouldHighlight 
                ? highlightColor 
                : normalColor;
        }
        
        if (buttonText != null)
        {
            buttonText.color = shouldHighlight 
                ? Color.white 
                : normalTextColor;

            if (shouldHighlight)
            {
                buttonText.fontMaterial.EnableKeyword("GLOW_ON");
                buttonText.fontMaterial.SetFloat("_GlowPower", glowIntensity);
            }
            else
            {
                buttonText.fontMaterial.DisableKeyword("GLOW_ON");
            }
        }
   
        if (outlineImage != null)
        {
            outlineImage.enabled = shouldHighlight;
        }
    }

    public void ResetVisuals()
    {
        isSelected = false;
        isHovered = false;
        UpdateVisuals();
    }
}
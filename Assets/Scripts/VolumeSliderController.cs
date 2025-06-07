using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class VolumeSliderController : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("References")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Image sliderHandle;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private GameObject confirmPrompt;
    
    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = new Color(1f, 0.9f, 0.5f);
    [SerializeField] private Color activeColor = new Color(0.5f, 1f, 0.5f);
    
    private bool isSelected = false;
    private bool isActive = false;
    private float lastVolume = 0f;
    private Navigation originalNavigation;
    
    private float initialValue;
    private bool blockValueChange = true;
    
    private void Start()
    {
        originalNavigation = volumeSlider.navigation;
        initialValue = volumeSlider.value;
        
        if (confirmPrompt != null)
            confirmPrompt.SetActive(false);
            
        volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }
    
    private void OnSliderValueChanged(float value)
    {
        if (blockValueChange)
        {
            volumeSlider.SetValueWithoutNotify(initialValue);
        }
        else
        {
            UpdateValueText();
            AudioListener.volume = volumeSlider.value;
            PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
        }
    }
    
    private void Update()
    {
        if (isSelected)
        {
            if (!isActive)
            {
                if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    ActivateSlider();
                }
            }
            else
            {
                float h = Input.GetAxis("Horizontal");
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                    h = -0.5f;
                else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                    h = 0.5f;
                
                if (Mathf.Abs(h) > 0.2f)
                {
                    float newValue = volumeSlider.value;
                    newValue += h * Time.unscaledDeltaTime * 0.5f;
                    volumeSlider.value = Mathf.Clamp01(newValue);
                }
                if (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.Escape) || 
                    Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    DeactivateSlider();
                }
            }
        }
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
        
        if (confirmPrompt != null)
            confirmPrompt.SetActive(true);
            
        UpdateVisuals();
    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        if (isActive)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
            return;
        }
        
        isSelected = false;
        
        if (confirmPrompt != null)
            confirmPrompt.SetActive(false);
            
        UpdateVisuals();
        
        initialValue = volumeSlider.value;
    }
    
    private void ActivateSlider()
    {
        isActive = true;
        blockValueChange = false;
        
        lastVolume = volumeSlider.value;
        initialValue = volumeSlider.value;
        
        Navigation noNav = new Navigation();
        noNav.mode = Navigation.Mode.None;
        volumeSlider.navigation = noNav;
        
        if (confirmPrompt != null && confirmPrompt.GetComponent<TextMeshProUGUI>() != null)
        {
            confirmPrompt.GetComponent<TextMeshProUGUI>().text = "Press Enter/B to confirm";
        }
        
        UpdateVisuals();
    }
    
    private void DeactivateSlider()
    {
        isActive = false;
        blockValueChange = true;
        
        initialValue = volumeSlider.value;
        
        StartCoroutine(EnableInteractionAfterDelay());
        
        volumeSlider.navigation = originalNavigation;
        
        if (confirmPrompt != null && confirmPrompt.GetComponent<TextMeshProUGUI>() != null)
        {
            confirmPrompt.GetComponent<TextMeshProUGUI>().text = "Press Enter/A to edit";
        }
        
        UpdateVisuals();
    }
    
    private System.Collections.IEnumerator EnableInteractionAfterDelay()
    {
        volumeSlider.interactable = false;
        yield return new WaitForSecondsRealtime(0.2f);
        volumeSlider.interactable = true;
    }
    
    private void UpdateVisuals()
    {
        Color currentColor = normalColor;
        
        if (isActive)
            currentColor = activeColor;
        else if (isSelected)
            currentColor = selectedColor;
            
        if (sliderHandle != null)
            sliderHandle.color = currentColor;
    }
    
    private void UpdateValueText()
    {
        if (valueText != null)
        {
            int percentage = Mathf.RoundToInt(volumeSlider.value * 100);
            valueText.text = percentage + "%";
        }
    }
    
    public bool IsMouseControl()
    {
        return !blockValueChange;
    }
}
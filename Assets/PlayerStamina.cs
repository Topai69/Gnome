using UnityEngine; 
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    public Slider staminaBar;  
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 20f;  
    public float staminaRegenRate = 10f; 
    public KeyCode sprintKey = KeyCode.LeftShift; // This is the sprint key btw

    private bool isSprinting = false;

    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = currentStamina;
    }

    void Update()
    {
        
        if (Input.GetKey(sprintKey) && currentStamina > 0) // Sprint logic here
        {
            isSprinting = true;
            currentStamina -= staminaDrainRate * Time.deltaTime;
        }
        else
        {
            isSprinting = false;
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
            }
        }

        
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); // Basically clamping stamina so it doesn't go negative
        staminaBar.value = currentStamina;
    }
}

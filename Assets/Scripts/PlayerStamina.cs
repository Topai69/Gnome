using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Added this for coroutine support

public class PlayerStamina : MonoBehaviour
{
    public Slider staminaBar;  
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 20f;  
    public float staminaRegenRate = 10f; 
    public KeyCode sprintKey = KeyCode.LeftShift; // This is the sprint key btw

    private bool isSprinting = false;

    public bool sprintAllowed = true; // New bool to control sprinting availability

    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = currentStamina;
    }

    void Update()
{
    // Sprinting only works if: key is held, stamina is available, sprint is allowed, and player is grounded
    if ((Input.GetKey(sprintKey) || Input.GetKey(KeyCode.JoystickButton0)) && currentStamina > 0f && sprintAllowed && GetComponent<PlayerMovement>().grounded)
    {
        isSprinting = true;
        currentStamina -= staminaDrainRate * Time.deltaTime;

        if (currentStamina <= 0f) // Added trigger disable coroutine when stamina depleats completely (hits 0)
        {
            currentStamina = 0f;
            StartCoroutine(DisableSprintTemporarily());
        }
    }
    else
    {
        isSprinting = false;
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
        }
    }

    currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    staminaBar.value = currentStamina;
}

    // Also added coroutine to disable sprinting temporarily (3 seconds) when stamina hits zero
    IEnumerator DisableSprintTemporarily()
    {
        sprintAllowed = false;
        yield return new WaitForSeconds(3f);
        sprintAllowed = true;
    }
}
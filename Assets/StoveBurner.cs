using UnityEngine;

public class StoveBurner : MonoBehaviour
{
    public float heatTime = 3f;
    public float launchForce = 20f;
    public string playerTag = "Player";

    [Header("Steam Effect")]
    public GameObject steamEffectPrefab; // Assign this in the Inspector

    [Header("Sound Effect")]
    public AudioClip launchSound; // 🔊 Assign your launch sound here
    private AudioSource audioSource;

    private bool playerOnBurner = false;
    private float heatTimer = 0f;
    private PlayerMovement playerMovement;

    void Start()
    {
        // Create an AudioSource component dynamically
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerOnBurner = true;
            playerMovement = other.GetComponent<PlayerMovement>();
            heatTimer = 0f;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (playerOnBurner && other.CompareTag(playerTag))
        {
            if (playerMovement != null && playerMovement.grounded)
            {
                heatTimer += Time.deltaTime;

                if (heatTimer >= heatTime)
                {
                    Rigidbody rb = other.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        // Reset Y velocity and apply launch
                        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                        rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);

                        // Spawn steam effect and parent it to the player's "butt"
                        if (steamEffectPrefab != null)
                        {
                            GameObject steamFX = Instantiate(steamEffectPrefab, other.transform);
                            steamFX.transform.localPosition = new Vector3(0f, -0.9f, 0f); // Adjust Y if needed
                            ParticleSystem ps = steamFX.GetComponent<ParticleSystem>();
                            if (ps != null)
                            {
                                ps.Play();
                            }
                            Destroy(steamFX, 3f); // Auto destroy after 3 seconds
                        }

                        // Play sound when launch occurs
                        if (launchSound != null)
                        {
                            audioSource.PlayOneShot(launchSound);
                        }
                    }

                    heatTimer = 0f;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerOnBurner = false;
            heatTimer = 0f;
            playerMovement = null;
        }
    }
}

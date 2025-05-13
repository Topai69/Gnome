using UnityEngine;

public class StoveBurner : MonoBehaviour
{
    public float heatTime = 3f;
    public float launchForce = 20f;
    public string playerTag = "Player";

    [Header("Steam Effect")]
    public GameObject steamEffectPrefab; 

    [Header("Sound Effect")]
    public AudioClip launchSound; 
    private AudioSource audioSource;

    private bool playerOnBurner = false;
    private float heatTimer = 0f;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerOnBurner = true;
            heatTimer = 0f;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (playerOnBurner && other.CompareTag(playerTag))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();

            if (rb != null && playerMovement != null && playerMovement.grounded)
            {
                heatTimer += Time.deltaTime;

                if (heatTimer >= heatTime)
                {
                    
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                    rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);

                    
                    playerMovement.hasJumped = true;

                    
                    if (steamEffectPrefab != null)
                    {
                        GameObject steamFX = Instantiate(steamEffectPrefab, other.transform);
                        steamFX.transform.localPosition = new Vector3(0f, -0.9f, 0f);
                        ParticleSystem ps = steamFX.GetComponent<ParticleSystem>();
                        if (ps != null) ps.Play();
                        Destroy(steamFX, 3f);
                    }

                    
                    if (launchSound != null)
                    {
                        audioSource.PlayOneShot(launchSound);
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
        }
    }
}

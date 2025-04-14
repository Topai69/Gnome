using UnityEngine;

public class StoveHeatFeedback : MonoBehaviour
{
    public Renderer stoveRenderer;
    public Color coldColor = Color.gray;
    public Color hotColor = Color.red;
    public float heatTime = 3f;

    private float heatProgress = 0f;
    private bool playerOnStove = false;
    private PlayerMovement playerMovement;

    void Update()
    {
        if (playerOnStove && playerMovement != null && playerMovement.grounded)
        {
            heatProgress += Time.deltaTime;
            float t = Mathf.Clamp01(heatProgress / heatTime);

            Color currentColor = Color.Lerp(coldColor, hotColor, t);
            stoveRenderer.material.SetColor("_BaseColor", currentColor);
            stoveRenderer.material.SetColor("_EmissionColor", currentColor * 2f);
        }
        else
        {
            if (heatProgress > 0f)
            {
                heatProgress -= Time.deltaTime * 2f;
                float t = Mathf.Clamp01(heatProgress / heatTime);
                Color currentColor = Color.Lerp(coldColor, hotColor, t);
                stoveRenderer.material.SetColor("_BaseColor", currentColor);
                stoveRenderer.material.SetColor("_EmissionColor", currentColor * 2f);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnStove = true;
            playerMovement = other.GetComponent<PlayerMovement>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnStove = false;
            playerMovement = null;
        }
    }
}

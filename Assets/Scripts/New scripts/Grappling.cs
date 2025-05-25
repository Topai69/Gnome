using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    public Transform cam;
    public Transform gunTip;
   
    public LayerMask whatIsGround;
    public LayerMask Interactable;

    [Header("Grappling")]

    public float maxGrappleDistance;
    public float grappleDelayTime;

    private bool grappling = false;

    [Header("Cooldown")]

    private Vector3 grapplePoint;


    private LineRenderer lineRenderer;

    public float grappleCooldown;
    private float grapplingCooldownTimer;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1)) StartGrapple();

        if (grappleCooldown > 0) grappleCooldown -= Time.deltaTime;
    }

    private void StartGrapple()
    {
        if (grapplingCooldownTimer > 0) return;

        grappling = true;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGround) || Physics.Raycast(cam.position, hit, maxGrappleDistance, Interactable)) // make two layers interactable
        {
            grapplePoint = hit.point;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(1, grapplePoint);
    }

    private void LateUpdate()
    {
        if (grappling)
        {
            lineRenderer.SetPosition(0, gunTip.position);
        }
    }

    private void ExecuteGrapple()
    {

    }

    private void StopGrapple()
    {
        grappling = false;
        grapplingCooldownTimer = grappleCooldown;

        lineRenderer.enabled = false;
    }
}

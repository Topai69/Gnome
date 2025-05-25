using Unity.VisualScripting;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    private LineRenderer rndr;
    private Vector3 grapplePoint;
    public LayerMask Grappable;
    public Transform gunTip, cam, player;
    private float maxDistance = 20f;
    private SpringJoint joint;



    private void Awake()
    {
        rndr = GetComponent<LineRenderer>();

    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0)) StartGrapple();
        else if (Input.GetMouseButtonUp(0)) StopGrapple();

    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxDistance, Grappable))
        {
            grapplePoint = hit.point;
            joint = player.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distance = Vector3.Distance(player.position, grapplePoint);
            joint.maxDistance = distance * 0.8f;
            joint.minDistance = distance * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;
        }
    }


    private void DrawRope()
    {
        if (!joint) return;
        rndr.SetPosition(0, gunTip.position);
        rndr.SetPosition(0, grapplePoint);
    }

    private void StopGrapple()
    {

    }

}

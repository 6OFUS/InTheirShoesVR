using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Wheelchair : MonoBehaviour
{
    public Rigidbody wheelchairRigidbody;
    public XRGrabInteractable leftWheel;
    public XRGrabInteractable rightWheel;
    public float pushForce = 5f;
    public float turnForce = 2f;

    private bool leftGrabbed = false;
    private bool rightGrabbed = false;
    private Transform leftGrabPoint;
    private Transform rightGrabPoint;

    private void Start()
    {
        // Get grab points (assumed to be children of the wheelchair)
        leftGrabPoint = leftWheel.transform;
        rightGrabPoint = rightWheel.transform;

        // Ensure Rigidbody settings for stability
        wheelchairRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Set wheels to kinematic so they don't detach
        Rigidbody leftRB = leftWheel.GetComponent<Rigidbody>();
        Rigidbody rightRB = rightWheel.GetComponent<Rigidbody>();
        leftRB.isKinematic = true;
        rightRB.isKinematic = true;

        // Add event listeners
        leftWheel.firstSelectEntered.AddListener(_ => leftGrabbed = true);
        leftWheel.lastSelectExited.AddListener(_ => leftGrabbed = false);
        rightWheel.firstSelectEntered.AddListener(_ => rightGrabbed = true);
        rightWheel.lastSelectExited.AddListener(_ => rightGrabbed = false);
    }
    public void MoveWheelchair()
    {

    }
    private void FixedUpdate()
    {
        // Ensure grab points stay at their correct positions relative to the wheelchair
        leftGrabPoint.position = transform.position + transform.right * -0.4f; // Adjust offset if needed
        rightGrabPoint.position = transform.position + transform.right * 0.4f;

        if (leftGrabbed && rightGrabbed)
        {
            // Move straight when both wheels are grabbed
            wheelchairRigidbody.AddForce(transform.forward * pushForce, ForceMode.Impulse);
        }
        else if (leftGrabbed)
        {
            // Grab left wheel → turn right
            wheelchairRigidbody.AddTorque(Vector3.up * turnForce, ForceMode.Impulse);
        }
        else if (rightGrabbed)
        {
            // Grab right wheel → turn left
            wheelchairRigidbody.AddTorque(Vector3.up * -turnForce, ForceMode.Impulse);
        }
    }
}

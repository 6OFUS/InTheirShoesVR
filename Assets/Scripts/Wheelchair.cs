/*
* Author: Jennifer Lau
* Date: 15/2/2025
* Description: Wheelchair script that handles the wheelchair movement ingame
* */
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Wheelchair : MonoBehaviour
{
    /// <summary>
    /// The Rigidbody component attached to the wheelchair for physics interactions.
    /// </summary>
    public Rigidbody wheelchairRigidbody;

    /// <summary>
    /// The XRGrabInteractable component for the left wheel, allowing the player to interact with it.
    /// </summary>
    public XRGrabInteractable leftWheel;

    /// <summary>
    /// The XRGrabInteractable component for the right wheel, allowing the player to interact with it.
    /// </summary>
    public XRGrabInteractable rightWheel;

    /// <summary>
    /// The force applied to move the wheelchair forward when both wheels are grabbed.
    /// </summary>
    public float pushForce = 5f;

    /// <summary>
    /// The force applied to rotate the wheelchair when one of the wheels is grabbed.
    /// </summary>
    public float turnForce = 2f;

    private bool leftGrabbed = false;
    private bool rightGrabbed = false;
    private Transform leftGrabPoint;
    private Transform rightGrabPoint;


    /// <summary>
    /// Called at the start of the script. Initializes grab points, wheel constraints, and event listeners.
    /// </summary>
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

    /// <summary>
    /// Moves the wheelchair based on the user's interaction with the left and right wheels.
    /// </summary>
    public void MoveWheelchair()
    {

    }
    /// <summary>
    /// Called every physics frame. Handles the movement and turning of the wheelchair based on grab states.
    /// </summary>
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

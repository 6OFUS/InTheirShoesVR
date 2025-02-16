/*
    Author: Kevin Heng
    Date: 14/2/2025
    Description: The Cane class is used to handle the interactions while using the white cane
*/
using Oculus.Haptics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Cane : MonoBehaviour
{
    /// <summary>
    /// Haptic feedback when hit metal pole
    /// </summary>
    [Header("Haptic clips")]
    public HapticClip metalPoleHaptic;
    /// <summary>
    /// Haptic feedback when hit metal rail
    /// </summary>
    public HapticClip metalRailingHaptic;
    /// <summary>
    /// Haptic feedback when hit plastic object
    /// </summary>
    public HapticClip plasticHaptic;
    /// <summary>
    /// Haptic feedback when hit ground
    /// </summary>
    public HapticClip hitGroundHaptic;
    /// <summary>
    /// Haptic feedback when hit NPC
    /// </summary>
    public HapticClip hitNPCHaptic;

    /// <summary>
    /// Audio when pick up white cane
    /// </summary>
    [Header("Audio")]
    public AudioSource pickUpCane;

    /// <summary>
    /// Haptic clip player for hitting metal pole
    /// </summary>
    private HapticClipPlayer metalPoleHapticClipPlayer;
    /// <summary>
    /// Haptic clip player for hitting metal rail
    /// </summary>
    private HapticClipPlayer metalRailingHapticClipPlayer;
    /// <summary>
    /// Haptic clip player for hitting plastic
    /// </summary>
    private HapticClipPlayer plasticHapticClipPlayer;
    /// <summary>
    /// Haptic clip player for hitting ground
    /// </summary>
    private HapticClipPlayer hitGroundHapticClipPlayer;
    /// <summary>
    /// Haptic clip player for hitting NPC
    /// </summary>  
    private HapticClipPlayer hitNPCHapticClipPlayer;

    /// <summary>
    /// Reference the current XR controller interacting with this object
    /// </summary>
    private XRBaseInteractor currentInteractor;

    /// <summary>
    /// Check which objects white cane collides with
    /// </summary>
    /// <param name="collision">Objects with the specific tags</param>
    private void OnCollisionEnter(Collision collision)
    {
        AudioSource audioSource = collision.gameObject.GetComponent<AudioSource>();
        if (collision.gameObject.GetComponent<AudioSource>() != null)
        {
            audioSource.Play();
            if (collision.gameObject.CompareTag("Metal pole"))
            {
                CollisionHapticFeedback(metalPoleHapticClipPlayer);
            }
            else if (collision.gameObject.CompareTag("Metal railing"))
            {
                CollisionHapticFeedback(metalRailingHapticClipPlayer);
            }
            else if (collision.gameObject.CompareTag("Plastic"))
            {
                CollisionHapticFeedback(plasticHapticClipPlayer);
            }
            else if (collision.gameObject.CompareTag("Ground"))
            {
                CollisionHapticFeedback(hitGroundHapticClipPlayer);
            }
            else if (collision.gameObject.CompareTag("NPC"))
            {
                CollisionHapticFeedback(hitNPCHapticClipPlayer);
            }
        }
    }
    /// <summary>
    /// Play the haptic feedback upon hitting an object
    /// </summary>
    /// <param name="hapticClipPlayer">Haptic clip player corresponding to the object hit</param>
    public void CollisionHapticFeedback(HapticClipPlayer hapticClipPlayer)
    {
        if (currentInteractor.handedness == InteractorHandedness.Left)
        {
            hapticClipPlayer.Play(Controller.Left);
        }
        else if (currentInteractor.handedness == InteractorHandedness.Right)
        {
            hapticClipPlayer.Play(Controller.Right);
        }
    }
    /// <summary>
    /// Reference which XR controller grabbed the white cane
    /// </summary>
    /// <param name="args">Event interactions in XR Grab Interactable</param>
    private void OnGrab(SelectEnterEventArgs args)
    {
        currentInteractor = args.interactorObject as XRBaseInteractor;
        pickUpCane.Play();
    }
    /// <summary>
    /// Remove the XR controller reference
    /// </summary>
    /// <param name="args">Event interactions in XR Grab Interactable</param>
    private void OnRelease(SelectExitEventArgs args)
    {
        currentInteractor = null;
    }

    private void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();

        // Subscribe to grab events
        if (grabInteractable)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }
    /// <summary>
    /// Create new haptic clip players for each haptic clip
    /// </summary>
    private void Awake()
    {
        metalPoleHapticClipPlayer = new HapticClipPlayer(metalPoleHaptic);
        metalRailingHapticClipPlayer = new HapticClipPlayer(metalRailingHaptic);
        plasticHapticClipPlayer = new HapticClipPlayer(plasticHaptic);
        hitGroundHapticClipPlayer = new HapticClipPlayer(hitGroundHaptic);
        hitNPCHapticClipPlayer = new HapticClipPlayer(hitNPCHaptic);
    }
}

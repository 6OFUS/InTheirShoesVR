using Oculus.Haptics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Cane : MonoBehaviour
{
    public HapticClip metalPoleHaptic;
    public HapticClip metalRailingHaptic;
    public HapticClip plasticHaptic;
    public HapticClip hitGroundHaptic;

    private HapticClipPlayer metalPoleHapticClipPlayer;
    private HapticClipPlayer metalRailingHapticClipPlayer;
    private HapticClipPlayer plasticHapticClipPlayer;
    private HapticClipPlayer hitGroundHapticClipPlayer;

    private XRBaseInteractor currentInteractor;

    private void OnCollisionEnter(Collision collision)
    {
        AudioSource audioSource = collision.gameObject.GetComponent<AudioSource>();
        audioSource.Play();
        if (collision.gameObject.GetComponent<AudioSource>() != null)
        {
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
        }
    }

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
    private void OnGrab(SelectEnterEventArgs args)
    {
        currentInteractor = args.interactorObject as XRBaseInteractor;
    }

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

    private void Awake()
    {
        metalPoleHapticClipPlayer = new HapticClipPlayer(metalPoleHaptic);
        metalRailingHapticClipPlayer = new HapticClipPlayer(metalRailingHaptic);
        plasticHapticClipPlayer = new HapticClipPlayer(plasticHaptic);
        hitGroundHapticClipPlayer = new HapticClipPlayer(hitGroundHaptic);
    }
}

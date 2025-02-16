/*
* Author: Kevin Heng
* Date: 15/2/2025
* Description: This script handles the hand animation 
* */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHand : MonoBehaviour
{
    /// <summary>
    /// The input action for pinch animation, triggered when the user pinches.
    /// </summary>
    public InputActionProperty pinchAnimationAction;

    /// <summary>
    /// The input action for grip animation, triggered when the user grips.
    /// </summary>
    public InputActionProperty gripAnimationAction;

    /// <summary>
    /// The Animator component responsible for controlling hand animations.
    /// </summary>
    public Animator handAnimator;

    /// <summary>
    /// Updates the hand animation parameters each frame based on user input.
    /// </summary>
    void Update()
    {
        // Read the pinch action value and set the "Trigger" parameter in the Animator
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", triggerValue);

        // Read the grip action value and set the "Grip" parameter in the Animator
        float gripValue = gripAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", gripValue);
    }

}

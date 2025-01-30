/*
    Author: https://youtu.be/pI8l42F6ZVc?si=HCqK-3r5V3RpM4z3
    Date: 30/1/2025
    Description: The HandAnimatorController class is used to handle the animations of the hand models which are the controllers
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimatorController : MonoBehaviour
{
    [SerializeField] private InputActionProperty triggerAction;
    [SerializeField] private InputActionProperty gripAction;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float triggerValue = triggerAction.action.ReadValue<float>();
        float gripValue= gripAction.action.ReadValue<float>();

        animator.SetFloat("Trigger", triggerValue);
        animator.SetFloat("Grip", gripValue);
    }
}

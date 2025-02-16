/*
* Author: Kevin Heng
* Date: 15/2/2025
* Description: Door exit script
* */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    // private void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log(" Exit ");
    //     Application.Quit();
    // }

    /// <summary>
    /// Handles the event when an object exits the trigger collider.
    /// Exits the application when triggered.
    /// </summary>
    /// <param name="other">The collider of the object exiting the trigger area.</param>
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(" Exit ");
        Application.Quit();
    }
}

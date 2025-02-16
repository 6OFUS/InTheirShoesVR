/*
* Author: Alfred Kang
* Date: 15/2/2025
* Description: Camera script for snapping pics
* */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSubject : MonoBehaviour
{
    /// <summary>
    /// Reference to the MessagesController for handling in-game messages.
    /// </summary>
    public MessagesController messagesController;

    /// <summary>
    /// Virtual method that gets triggered when the subject is snapped by the camera.
    /// </summary>
    public virtual void Snapped()
    {
        
    }
}

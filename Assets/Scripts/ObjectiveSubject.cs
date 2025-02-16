/*
* Author: Alfred Kang
* Date: 15/2/2025
* Description: Camera script for snapping pics
* */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveSubject : CameraSubject
{
    /// <summary>
    /// Snapping of objectives
    /// </summary>
    public override void Snapped()
    {
        messagesController.SendNextMessage();
        Debug.Log("Snapped Objective");
    }
}

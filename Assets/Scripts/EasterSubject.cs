/*
* Author: Alfred Kang
* Date: 15/2/2025
* Description: Camera snapping easter egg
* */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterSubject : CameraSubject
{
    /// <summary>
    /// Snapping of easter egg object
    /// </summary>
    public override void Snapped()
    {
        Debug.Log("Snapped Easter Egg!");
    }
}

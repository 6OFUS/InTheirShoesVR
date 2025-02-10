using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterSubject : CameraSubject
{
    public override void Snapped()
    {
        Debug.Log("Snapped Easter Egg!");
    }
}

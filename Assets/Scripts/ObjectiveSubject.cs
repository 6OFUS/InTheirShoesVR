using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveSubject : CameraSubject
{
    public override void Snapped()
    {
        Debug.Log("Snapped Objective");
    }
}

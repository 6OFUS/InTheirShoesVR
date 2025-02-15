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

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(" Exit ");
        Application.Quit();
    }
}

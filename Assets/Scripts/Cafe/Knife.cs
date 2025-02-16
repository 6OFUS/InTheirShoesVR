/*
* Author: Jennifer Lau
* Date: 15/2/2025
* Description: Knife script for cutting mechanic
* */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    /// <summary>
    /// Triggered when the knife collides with another object.
    /// If the collided object has a CutFruit component, it calls the Cut method on that object.
    /// </summary>
    /// <param name="other">The collider of the object that the knife collides with.</param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CutFruit>() != null)
        {
            CutFruit cutFruit = other.GetComponent<CutFruit>();
            cutFruit.Cut();
        }
    }
}

/*
* Author: Jennifer Lau
* Date: 15/2/2025
* Description: Fruit cutting script
* */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutFruit : MonoBehaviour
{
    /// <summary>
    /// The whole fruit object before it is cut.
    /// </summary>
    public GameObject wholeFruit;

    /// <summary>
    /// The array of fruit slices that appear after the fruit is cut.
    /// </summary>
    public GameObject[] cutFruits;

    /// <summary>
    /// The particle system that plays when the fruit is cut.
    /// </summary>
    public ParticleSystem splash;

    /// <summary>
    /// Cuts the fruit by playing the cutting particle effect, disabling the whole fruit, 
    /// and enabling the sliced fruit pieces.
    /// </summary>
    public void Cut()
    {
        splash.Play();
        wholeFruit.SetActive(false);
        foreach(GameObject slice in cutFruits)
        {
            slice.SetActive(true);
        }
    }
}

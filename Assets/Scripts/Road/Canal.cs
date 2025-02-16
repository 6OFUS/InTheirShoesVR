/*
    Author: Kevin Heng
    Date: 31/1/2025
    Description: The Canal class is used to handle the restarting of scene when player walks into that area
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Canal : MonoBehaviour
{
    /// <summary>
    /// Reference SceneTransitionManager class
    /// </summary>
    public SceneTransitionManager transitionManager;

    /// <summary>
    /// Function to restart scene when player enters trigger area
    /// </summary>
    /// <param name="other">Player</param>
    private void OnTriggerEnter(Collider other)
    {
        transitionManager.ChangeSceneAsyc(SceneManager.GetActiveScene().buildIndex);
    }
}

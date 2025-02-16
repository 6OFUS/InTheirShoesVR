/*
    Author: Kevin Heng
    Date: 31/1/2025
    Description: Kiosk class is used to spawn the keycards for the player
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KioskButton : MonoBehaviour
{
    /// <summary>
    /// Reference keycard class
    /// </summary>
    public GameObject keycard;
    /// <summary>
    /// Reference spawnPoint class
    /// </summary>
    public GameObject spawnPoint;

    /// <summary>
    /// Reference MessageController class
    /// </summary>
    MessagesController messagesController;

    private void Start()
    {
        messagesController = FindObjectOfType<MessagesController>();
    }

    /// <summary>
    /// Instantiates a keycard at the designated spawn point.
    /// </summary>
    public void KeycardDispense()
    {
        Instantiate(keycard, spawnPoint.transform.position, spawnPoint.transform.rotation);
        if (!GameManager.Instance.playerLevelProgress["Dyslexic"].completed)
        {
            messagesController.SendNextMessage();
        }
    }
}

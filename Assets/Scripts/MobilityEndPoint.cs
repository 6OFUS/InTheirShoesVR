/*
* Author: Jennifer Lau
* Date: 15/2/2025
* Description: Mobility scene script that handles the end point
* */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilityEndPoint : MonoBehaviour
{
    /// <summary>
    /// Reference to the database manager for updating level completion and player points.
    /// </summary>
    Database database;

    /// <summary>
    /// The name of the current level that is being completed.
    /// </summary>
    public string currentLevelName;

    /// <summary>
    /// The audio source used to play sound effects when the level is completed.
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// The messages controller responsible for sending messages to the player.
    /// </summary>
    private MessagesController messagesController;

    /// <summary>
    /// The points awarded for completing this level.
    /// </summary>
    private int levelPoints = 500;

    /// <summary>
    /// Reference to the reset button that will be disabled upon level completion.
    /// </summary>
    public GameObject resetButton;

    /// <summary>
    /// This method is called when the player enters the end point trigger.
    /// It updates the player's progress, awards points, and triggers sound and messages.
    /// </summary>
    /// <param name="other">The collider of the object that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            database.UpdateLevelComplete(GameManager.Instance.playerID, currentLevelName, true, "A4", DateTime.UtcNow.ToString("yyyy-MM-dd"), true);
            database.UpdatePlayerPoints(GameManager.Instance.playerID, GameManager.Instance.playerPoints + levelPoints);
            audioSource.Play();
            StartCoroutine(messagesController.SendMultipleMessages(2, 2));
            resetButton.SetActive(false);
        }
    }

    /// <summary>
    /// Called at the start of the scene. Initializes necessary components and sends the first message.
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        database = FindObjectOfType<Database>();
        audioSource = GetComponent<AudioSource>();
        messagesController = FindObjectOfType<MessagesController>();
        StartCoroutine(messagesController.SendMultipleMessages(0,1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

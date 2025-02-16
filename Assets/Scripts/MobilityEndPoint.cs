using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilityEndPoint : MonoBehaviour
{
    Database database;
    public string currentLevelName;
    private AudioSource audioSource;
    private MessagesController messagesController;
    private int levelPoints = 500;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            database.UpdateLevelComplete(GameManager.Instance.playerID, currentLevelName, true, "A4", DateTime.UtcNow.ToString("yyyy-MM-dd"), true);
            database.UpdatePlayerPoints(GameManager.Instance.playerID, GameManager.Instance.playerPoints + levelPoints);
            audioSource.Play();
            StartCoroutine(messagesController.SendMultipleMessages(2, 2));
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        database = FindObjectOfType<Database>();
        audioSource = GetComponent<AudioSource>();
        messagesController = FindObjectOfType<MessagesController>();
        StartCoroutine(messagesController.SendMultipleMessages(0,2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

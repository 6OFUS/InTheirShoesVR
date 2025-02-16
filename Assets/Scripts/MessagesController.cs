/*
* Author: Hui Hui
* Date: 15/2/2025
* Description: MessagesController that handles the messages sent to player
* */
using System.Collections;
using UnityEngine;
using TMPro;

public class MessagesController : MonoBehaviour
{
    /// <summary>
    /// The prefab used to display messages in the UI
    /// </summary>
    public GameObject messagePrefab;

    /// <summary>
    /// The parent transform in which the messages will be instantiated (usually a scroll view)
    /// </summary>
    public Transform messagesScrollView;

    /// <summary>
    /// Array of predefined messages to send to the player
    /// </summary>
    public string[] messages;

    /// <summary>
    /// The index of the current message to be sent
    /// </summary>
    private int currentMessageIndex = 0;

    /// <summary>
    /// The audio source component used to play sound when a message is sent
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// Sends the next message from the messages array to the player.
    /// </summary>
    public void SendNextMessage()
    {
        if (currentMessageIndex >= messages.Length)
        {
            Debug.LogWarning("No more messages to send!");
            return;
        }

        Debug.Log("Sending Message: " + messages[currentMessageIndex]);
        GameObject newMessage = Instantiate(messagePrefab, messagesScrollView);

        TextMeshProUGUI textComponent = newMessage.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            audioSource.Play();
            textComponent.text = messages[currentMessageIndex];
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found in messagePrefab!");
        }

        currentMessageIndex++;
    }

    /// <summary>
    /// Sends a custom message to the player.
    /// </summary>
    /// <param name="message">The message to send</param>
    public void SendCustomMessage(string message)
    {
        GameObject newMessage = Instantiate(messagePrefab, messagesScrollView);

        TextMeshProUGUI textComponent = newMessage.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            audioSource.Play();
            textComponent.text = message;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found in messagePrefab!");
        }
    }


    /// <summary>
    /// Sends multiple messages to the player starting from a given index.
    /// </summary>
    /// <param name="startIndex">The index of the first message to send</param>
    /// <param name="messagesNum">The number of messages to send</param>
    /// <returns>A coroutine for sending multiple messages</returns>
    public IEnumerator SendMultipleMessages(int startIndex, int messagesNum)
    {
        int endIndex = Mathf.Min(startIndex + messagesNum, messages.Length);

        for (int i = startIndex; i < endIndex; i++)
        {
            SendNextMessage();
            yield return new WaitForSecondsRealtime(2f);

        }
    }

    /// <summary>
    /// Called on the start of the game. Initializes the audio source and sends the first message.
    /// </summary>
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SendNextMessage();     
    }

}
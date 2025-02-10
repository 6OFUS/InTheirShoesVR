using System.Collections;
using UnityEngine;
using TMPro;

public class MessagesController : MonoBehaviour
{
    public GameObject messagePrefab;
    public Transform messagesScrollView;
    public string[] messages;
    private int currentMessageIndex = 0;

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
            textComponent.text = messages[currentMessageIndex];
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found in messagePrefab!");
        }

        currentMessageIndex++;
    }

    public void SendCustomMessage(string message)
    {
        GameObject newMessage = Instantiate(messagePrefab, messagesScrollView);

        TextMeshProUGUI textComponent = newMessage.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = message;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found in messagePrefab!");
        }
    }
    
    IEnumerator SendMessagesTest()
    {
        for (int i = 0; i < messages.Length; i++)
        {
            SendNextMessage();
            yield return new WaitForSeconds(2f);
        }
    }
    public void Start()
    {
        StartCoroutine(SendMessagesTest());
    }
}
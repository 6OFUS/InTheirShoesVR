using System.Collections;
using UnityEngine;
using TMPro;

public class MessagesController : MonoBehaviour
{
    public GameObject messagePrefab;
    public Transform messagesScrollView;
    public string[] messages;
    private int currentMessageIndex = 0;
    private AudioSource audioSource;

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

    public IEnumerator SendMultipleMessages(int startIndex, int messagesNum)
    {
        int endIndex = Mathf.Min(startIndex + messagesNum, messages.Length);

        for (int i = startIndex; i < endIndex; i++)
        {
            SendNextMessage();
            yield return new WaitForSecondsRealtime(2f);

        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SendNextMessage();     
    }

}
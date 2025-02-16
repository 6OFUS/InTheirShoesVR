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
            textComponent.text = message;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found in messagePrefab!");
        }
    }

    public IEnumerator SendMultipleMessages(int messagesNum)
    {
        for(int i = 0; i <  messagesNum; i++)
        {
            SendNextMessage();
            yield return new WaitForSeconds(2f);
        }
    }
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(GameManager.Instance.playerID == "")
        {
            SendNextMessage();
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KioskButton : MonoBehaviour
{
    public GameObject keycard;
    public GameObject spawnPoint;

    MessagesController messagesController;

    private void Start()
    {
        messagesController = FindObjectOfType<MessagesController>();
    }
    public void KeycardDispense()
    {
        Instantiate(keycard, spawnPoint.transform.position, spawnPoint.transform.rotation);
        if (!GameManager.Instance.playerLevelProgress["Dyslexic"].completed)
        {
            messagesController.SendNextMessage();
        }
    }
}

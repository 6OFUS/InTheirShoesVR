using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordChecker : MonoBehaviour
{
    public string[] correctWords;
    public List<LetterSocketGroup> letterSocketGroups;
    Database database;
    public string currentLevelName;
    public GameObject levelCompleteUI;

    public void CheckWord()
    {
        /*
        string formedWord = "";

        foreach (var socket in sockets)
        {
            formedWord += socket.GetLetter();
        }

        if (formedWord == correctWord)
        {
            Debug.Log($"{formedWord} is the Correct word!");
            levelCompleteUI.SetActive(true);
            database.UpdateLevelComplete(GameManager.Instance.playerID, currentLevelName, true);
        }
        else
        {
            Debug.Log("Keep trying...");
        }
        */
    }
    // Start is called before the first frame update
    void Start()
    {
        database = FindObjectOfType<Database>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

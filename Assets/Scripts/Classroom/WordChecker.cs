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
    public int correctWordCounter;

    public void CheckWord()
    {
        correctWordCounter = 0;

        for (int i = 0; i < correctWords.Length; i++)
        {
            string formedWord = "";
            LetterSocketGroup group = letterSocketGroups[i];
            foreach(var socket in group.letterSockets)
            {
                formedWord += socket.GetLetter();
            }
            if (formedWord == correctWords[i])
            {
                correctWordCounter++;
                Debug.Log($"{formedWord} is the Correct word!");
                Debug.Log(correctWordCounter);
            }
            else
            {
                Debug.Log($"{formedWord} is the incorrect word!");
            }
        }
        if(correctWordCounter == correctWords.Length)
        {
            Debug.Log("All correct! Level completed");
            levelCompleteUI.SetActive(true);
            database.UpdateLevelComplete(GameManager.Instance.playerID, currentLevelName, true);
        }
        else
        {
            Debug.Log(correctWordCounter);
            Debug.Log("Check your answers again");
        }
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

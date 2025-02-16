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
    public AudioSource incorrectAns;
    public AudioSource correctAns;

    private int levelPoints = 500;
    private int retryCount;

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
            levelCompleteUI.SetActive(true);
            correctAns.Play();
            database.UpdateLevelComplete(GameManager.Instance.playerID, currentLevelName, true, "A2", DateTime.UtcNow.ToString("yyyy-MM-dd"), true);
            database.UpdatePlayerPoints(GameManager.Instance.playerID, CalculatePoints());
            Debug.Log(GameManager.Instance.playerPoints);
        }
        else
        {
            retryCount++;
            incorrectAns.Play();
        }
    }

    private int CalculatePoints()
    {
        levelPoints -= 10 * retryCount;
        GameManager.Instance.playerPoints += levelPoints;
        return GameManager.Instance.playerPoints;
    }

    // Start is called before the first frame update
    void Start()
    {
        database = FindObjectOfType<Database>();
    }


}

/*
* Author: Kevin Heng
* Date: 15/2/2025
* Description: Crossword Puzzle script
* */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordChecker : MonoBehaviour
{
    /// <summary>
    /// The array of correct words that need to be formed in the puzzle.
    /// </summary>
    public string[] correctWords;

    /// <summary>
    /// A list of LetterSocketGroup objects, where each group represents a set of letter sockets.
    /// </summary>
    public List<LetterSocketGroup> letterSocketGroups;

    /// <summary>
    /// The database instance used for updating player data and level completion.
    /// </summary>
    Database database;

    /// <summary>
    /// The name of the current level.
    /// </summary>
    public string currentLevelName;

    /// <summary>
    /// The UI element that shows when the level is complete.
    /// </summary>
    public GameObject levelCompleteUI;

    /// <summary>
    /// A counter for the correct words formed in the puzzle.
    /// </summary>
    public int correctWordCounter;

    /// <summary>
    /// Audio source for the incorrect answer sound effect.
    /// </summary>
    public AudioSource incorrectAns;

    /// <summary>
    /// Audio source for the correct answer sound effect.
    /// </summary>
    public AudioSource correctAns;

    /// <summary>
    /// The MessagesController instance used for sending messages.
    /// </summary>
    [SerializeField] private MessagesController messagesController;

    /// <summary>
    /// The base points awarded for completing the level.
    /// </summary>
    private int levelPoints = 500;

    /// <summary>
    /// A counter for the number of retries.
    /// </summary>
    private int retryCount;


    private DemoLevelChecker levelChecker;
    private bool pressedBell;


    /// <summary>
    /// Checks the words formed by the player against the correct words.
    /// </summary>
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
        if(correctWordCounter == correctWords.Length && !pressedBell)
        {
            pressedBell = true;
            correctAns.Play();
            /*
             * ---------------------------------------------- REMOVED FOR DEMO -----------------------------------------------------------------------------
             * levelCompleteUI.SetActive(true);
             * database.UpdateLevelComplete(GameManager.Instance.playerID, currentLevelName, true, "A2", DateTime.UtcNow.ToString("yyyy-MM-dd"), true);
             * database.UpdatePlayerPoints(GameManager.Instance.playerID, CalculatePoints());
             * Debug.Log(GameManager.Instance.playerPoints);
             */
            StartCoroutine(messagesController.SendMultipleMessages(3, 2));
            //----------------- FOR DEMO ---------------------
            levelChecker.levelCompleted[0] = true;
        }
        else
        {
            if (pressedBell)
            {
                correctAns.Play();
                return;
            }
            retryCount++;
            incorrectAns.Play();
        }
    }

    /// <summary>
    /// Calculates the points to be awarded based on the number of retries.
    /// </summary>
    /// <returns>The total points awarded to the player.</returns>
    private int CalculatePoints()
    {
        levelPoints -= 10 * retryCount;
        GameManager.Instance.playerPoints += levelPoints;
        return GameManager.Instance.playerPoints;
    }

    /// <summary>
    /// Initializes the WordChecker script, finds necessary references, and sends initial messages.
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        /*
         * ------ REMOVED FOR DEMO ---------------
         * messagesController = FindObjectOfType<MessagesController>();
         * database = FindObjectOfType<Database>();
        */
        StartCoroutine(messagesController.SendMultipleMessages(0,3));
        levelChecker = FindObjectOfType<DemoLevelChecker>();
    }
}

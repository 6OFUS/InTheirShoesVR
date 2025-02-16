/*
* Author: Malcom Goh
* Date: 15/2/2025
* Description: Game manager script that manages the game settings
* */
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the GameManager
    /// </summary>
    public static GameManager Instance;

    /// <summary>
    /// Reference to the Database class to store and retrieve data
    /// </summary>
    public Database database;

    /// <summary>
    /// Reference to the Authentication class to handle player authentication
    /// </summary>
    public Authentication authentication;

    /// <summary>
    /// The unique identifier for the player
    /// </summary>
    public string playerID;

    /// <summary>
    /// The email address of the player
    /// </summary>
    public string playerEmail;

    /// <summary>
    /// The name of the player
    /// </summary>
    public string playerName;

    /// <summary>
    /// The date the player joined the game
    /// </summary>
    public string playerDateJoined;

    /// <summary>
    /// The total playtime of the player in seconds
    /// </summary>
    public int playerPlayTimeSeconds;

    /// <summary>
    /// A flag indicating whether playtime tracking is enabled
    /// </summary>
    public bool isTracking;

    /// <summary>
    /// The total points accumulated by the player
    /// </summary>
    public int playerPoints;


    /// <summary>
    /// A dictionary to track the progress of the player in various levels
    /// </summary>
    public Dictionary<string, (bool completed, bool doorUnlocked)> playerLevelProgress = new Dictionary<string, (bool, bool)>();

    /// <summary>
    /// Called when the script is first loaded. Initializes the singleton instance.
    /// </summary>
    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this; 
            DontDestroyOnLoad(gameObject); 
        }
        else if (Instance != null && Instance != this) 
        {
            Destroy(gameObject); 
        }

    }

    /// <summary>
    /// Stores the details of the player such as ID, name, email, date joined, playtime, and points.
    /// </summary>
    /// <param name="uID">Player's unique identifier</param>
    /// <param name="name">Player's name</param>
    /// <param name="email">Player's email address</param>
    /// <param name="dateJoined">Date the player joined</param>
    /// <param name="playTime">Total playtime in seconds</param>
    /// <param name="points">Total points accumulated</param>
    public void StorePlayerDetails(string uID, string name, string email, string dateJoined, int playTime, int points)
    {
        playerID = uID;
        playerName = name;
        playerEmail = email;
        playerDateJoined = dateJoined;
        playerPlayTimeSeconds = playTime;
        playerPoints = points;
    }

    /// <summary>
    /// Tracks the player's playtime by incrementing the seconds and storing it every 30 seconds.
    /// </summary>
    /// <returns>A coroutine to track the playtime</returns>
    public IEnumerator TrackPlayTime()
    {
        while (isTracking)
        {
            yield return new WaitForSeconds(1);
            playerPlayTimeSeconds++;
            if (playerPlayTimeSeconds % 30 == 0)
            {
                database.StorePlayTime(playerID, playerPlayTimeSeconds);
            }
        }
    }

    /// <summary>
    /// Called when the application is about to quit. Stores playtime data and signs the player out.
    /// </summary>
    private void OnApplicationQuit()
    {
        if(playerID != "")
        {
            database.StorePlayTime(playerID, playerPlayTimeSeconds);
            authentication.Signout();
        }
    }
}

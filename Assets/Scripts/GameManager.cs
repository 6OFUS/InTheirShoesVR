using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Database database;
    public Authentication authentication;

    public string playerID;
    public string playerEmail;
    public string playerName;
    public string playerDateJoined;
    public int playerPlayTime;
    public bool isTracking;
    public int playerPoints;

    public Dictionary<string, (bool completed, bool doorUnlocked)> playerLevelProgress = new Dictionary<string, (bool, bool)>();

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
    public void StorePlayerDetails(string uID, string name, string email, string dateJoined, int playTime, int points)
    {
        playerID = uID;
        playerName = name;
        playerEmail = email;
        playerDateJoined = dateJoined;
        playerPlayTime = playTime;
        playerPoints = points;
    }

    public IEnumerator TrackPlayTime()
    {
        while (isTracking)
        {
            yield return new WaitForSeconds(60);
            playerPlayTime++;
            database.StorePlayTime(playerID, playerPlayTime);
        }
    }

    private void OnApplicationQuit()
    {
        database.StorePlayTime(playerID, playerPlayTime);
        authentication.Signout();
    }
}

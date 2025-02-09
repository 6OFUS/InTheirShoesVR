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
    public int playerPlayTimeSeconds;
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
        playerPlayTimeSeconds = playTime;
        playerPoints = points;
    }

    public IEnumerator TrackPlayTime()
    {
        while (isTracking)
        {
            yield return new WaitForSeconds(1);
            playerPlayTimeSeconds++;
            if(playerPlayTimeSeconds % 30 == 0)
            {
                database.StorePlayTime(playerID, playerPlayTimeSeconds);
            }
        }
    }

    private void OnApplicationQuit()
    {
        database.StorePlayTime(playerID, playerPlayTimeSeconds);
        authentication.Signout();
    }
}

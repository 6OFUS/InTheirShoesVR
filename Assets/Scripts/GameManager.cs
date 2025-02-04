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


    public Dictionary<string, bool> playerLevelProgress = new Dictionary<string, bool>();
    public int currentLevelIndex;


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
    public void StorePlayerDetails(string uID, string name, string email, string dateJoined)
    {
        playerID = uID;
        playerName = name;
        playerEmail = email;
        playerDateJoined = dateJoined;
    }

    public IEnumerator TrackPlayTime()
    {
        while (isTracking)
        {
            playerPlayTime++;
            if (playerPlayTime % 10 == 0)
            {
                database.StorePlayTime(playerID, playerPlayTime);
            }
            yield return new WaitForSeconds(1);
        }
    }

    private void OnApplicationQuit()
    {
        database.StorePlayTime(playerID, playerPlayTime);
        authentication.Signout();
    }
}

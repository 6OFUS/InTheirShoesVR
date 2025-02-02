using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using System.Threading.Tasks;
using System;

public class Database : MonoBehaviour
{
    public DatabaseReference dbRef;
    public List<string> levelNames;

    public void CreateNewPlayer(string uID, string name, string email, string dateJoined, int totalPlayTime)
    {
        // Create and store player data
        Player player = new Player(name, email, dateJoined, totalPlayTime);
        string playerJson = JsonUtility.ToJson(player);
        dbRef.Child("players").Child(uID).SetRawJsonValueAsync(playerJson);

        // Set all level progress
        PlayerLevelProgress playerLevelProgress = new PlayerLevelProgress(levelNames, false);

        // Loop through each level in the playerLevelProgress
        foreach (var level in playerLevelProgress.levelsProgress)
        {
            // Construct path for each level and set the player progress (completed: false)
            dbRef.Child("levelProgress").Child(level.Key).Child(uID).SetValueAsync(level.Value);
        }
    }

    public void UpdateLevelCompletion(string uID, string levelName, bool completedLevel)
    {
        DatabaseReference levelProgressRef = dbRef.Child("levelProgress").Child(levelName).Child(uID).Reference;
        levelProgressRef.SetValueAsync(completedLevel);
    }

    public void ReadPlayerData(string uID)
    {
        FirebaseDatabase.DefaultInstance.GetReference("players").Child(uID).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                //error message
                Debug.Log(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string playerData = snapshot.GetRawJsonValue();
                Player data = JsonUtility.FromJson<Player>(playerData);
                GameManager.Instance.StorePlayerDetails(
                    uID,
                    data.name,
                    data.email,
                    data.dateJoined,
                    data.totalPlayTime
                );
            }
        });
    }

    public void ReadPlayerLvlProgress(string uID)
    {
        foreach(var level in levelNames)
        {
            FirebaseDatabase.DefaultInstance.GetReference("levelProgress").Child(level).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    //error message
                    Debug.Log(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    foreach(var playerID in snapshot.Children)
                    {
                        if(playerID.Key == GameManager.Instance.playerID)
                        {
                            GameManager.Instance.playerLevelProgress.Add(level, (bool)playerID.Value);
                            break;
                        }
                    }
                    /*
                    foreach (KeyValuePair<string, bool> entry in GameManager.Instance.playerLevelProgress)
                    {
                        Debug.Log("Level: " + entry.Key + ", Completed: " + entry.Value);
                    }
                    */
                }
            });
        }
    }

    public void StorePlayTime(string uID, int playTime)
    {
        if(uID != "")
        {
            dbRef.Child("players").Child(uID).Child("totalPlayTime").SetValueAsync(playTime);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

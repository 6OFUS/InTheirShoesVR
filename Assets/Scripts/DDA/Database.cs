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
        Player player = new Player(
            name, 
            email, 
            DateTime.Parse(dateJoined), 
            totalPlayTime, 
            "default",
            "",
            ""
        );
        
        string playerJson = JsonUtility.ToJson(player);
        Debug.Log("Serialized Player JSON: " + playerJson);
        
        dbRef.Child("players").Child(uID).SetRawJsonValueAsync(playerJson);
    }


    public void UpdateLevelCompletion(string uID, string levelName, bool completedLevel)
    {
        DatabaseReference levelProgressRef = dbRef.Child("levelProgress").Child(levelName).Child(uID);
        levelProgressRef.SetValueAsync(completedLevel);
    }

    public void ReadPlayerData(string uID)
    {
        FirebaseDatabase.DefaultInstance.GetReference("players").Child(uID).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string playerData = snapshot.GetRawJsonValue();
                Player data = JsonUtility.FromJson<Player>(playerData);
                GameManager.Instance.StorePlayerDetails(
                    uID,
                    data.Name,
                    data.Email,
                    data.DateJoined.ToString("yyyy-MM-dd"),
                    data.TotalPlayTime
                );
                /* GameManager.Instance.SetPlayerCustomization(data.PreferredTheme, data.ProfilePictureUrl, data.PhotoGalleryPath);
                GameManager.Instance.LoadAchievements(data.Achievements);
                GameManager.Instance.UpdateAccessibilityProgress(data.Accessibility); */
            }
        });
    }

    public void ReadPlayerLvlProgress(string uID)
    {
        foreach (var level in levelNames)
        {
            FirebaseDatabase.DefaultInstance.GetReference("levelProgress").Child(level).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    foreach (var playerID in snapshot.Children)
                    {
                        if (playerID.Key == GameManager.Instance.playerID)
                        {
                            bool completed = (bool)playerID.Value;
                            GameManager.Instance.playerLevelProgress[level] = completed;
                            if (completed)
                            {
                                GameManager.Instance.currentLevelIndex++;
                                Debug.Log(GameManager.Instance.currentLevelIndex);
                            }
                            break;
                        }
                    }
                }
            });
        }
    }

    public void StorePlayTime(string uID, int playTime)
    {
        if (!string.IsNullOrEmpty(uID))
        {
            dbRef.Child("players").Child(uID).Child("totalPlayTime").SetValueAsync(playTime);
        }
    }

    void Start()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }
}

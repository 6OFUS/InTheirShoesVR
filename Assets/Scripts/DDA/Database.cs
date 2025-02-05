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
        
    
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            Firebase.DependencyStatus status = task.Result;
            if (status == Firebase.DependencyStatus.Available)
            {
                // Initialize Firebase services
                dbRef = FirebaseDatabase.DefaultInstance.RootReference;
                if (dbRef == null)
                {
                    Debug.LogError("Database reference is still null after initialization.");
                }
                else
                {
                    Debug.Log("Firebase initialized successfully, dbRef is ready.");
                }
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + status);
            }
        });
    }

    public void CreateNewPlayer(string uID, string name, string email, string dateJoined)
    {
        Player player = new Player(name, email, dateJoined, "default", "", "");

        string playerJson = JsonUtility.ToJson(player, true);
        Debug.Log("Serialized Player JSON: " + playerJson);

        if (dbRef == null)
        {
            Debug.LogError("Firebase dbRef is null!");
            return;
        }

        dbRef.Child("players").Child(uID).SetRawJsonValueAsync(playerJson)
            .ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Player data successfully pushed to Firebase.");
                }
                else if (task.IsFaulted)
                {
                    Debug.LogError("Error pushing data to Firebase: " + task.Exception);
                }
            });
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
                    data.DateJoined
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
}

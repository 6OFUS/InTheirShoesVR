using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using System.Threading.Tasks;
using System;
using static UnityEngine.Rendering.HighDefinition.ScalableSettingLevelParameter;

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
        Player player = new Player(name, email, dateJoined, "default", "", "", 0);

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
                int totalPlayTime = Convert.ToInt32(snapshot.Child("totalPlayTime").Value);
                string playerData = snapshot.GetRawJsonValue();
                Player data = JsonUtility.FromJson<Player>(playerData);
                GameManager.Instance.StorePlayerDetails(
                    uID,
                    data.Name,
                    data.Email,
                    data.DateJoined,
                    totalPlayTime,
                    data.Points
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
            FirebaseDatabase.DefaultInstance.GetReference("players").Child(uID).Child("Progress").Child(level).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    bool completed = (bool)snapshot.Child("Completed").Value;
                    bool doorUnlocked = (bool)snapshot.Child("DoorUnlocked").Value;
                    GameManager.Instance.playerLevelProgress[level] = (completed, doorUnlocked);
                    Debug.Log(GameManager.Instance.playerLevelProgress[level]);
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

    public void UpdateDoorLockStatus(string uID, string levelName, bool doorUnlocked)
    {
        FirebaseDatabase.DefaultInstance.GetReference("players").Child(uID).Child("Progress").Child(levelName).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Sending update");
                DataSnapshot snapshot = task.Result;
                bool completed = (bool)snapshot.Child("Completed").Value;
                GameManager.Instance.playerLevelProgress[levelName] = (completed, doorUnlocked);
                FirebaseDatabase.DefaultInstance.GetReference("players")
                .Child(uID)
                .Child("Progress")
                .Child(levelName)
                .Child("DoorUnlocked")  // Ensure this matches your Firebase structure
                .SetValueAsync(doorUnlocked).ContinueWithOnMainThread(updateTask =>
                {
                    if (updateTask.IsFaulted)
                    {
                        Debug.Log("Error updating DoorUnlocked in Firebase: " + updateTask.Exception);
                    }
                    else
                    {
                        Debug.Log("DoorUnlocked updated successfully in Firebase!");
                    }
                });
            }
        });
    }
    public void UpdateLevelComplete(string uID, string levelName, bool levelCompleted)
    {
        FirebaseDatabase.DefaultInstance.GetReference("players").Child(uID).Child("Progress").Child(levelName).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Sending update");
                DataSnapshot snapshot = task.Result;
                bool doorUnlocked = (bool)snapshot.Child("DoorUnlocked").Value;
                GameManager.Instance.playerLevelProgress[levelName] = (levelCompleted, doorUnlocked);
                FirebaseDatabase.DefaultInstance.GetReference("players")
                .Child(uID)
                .Child("Progress")
                .Child(levelName)
                .Child("Completed")  // Ensure this matches your Firebase structure
                .SetValueAsync(doorUnlocked).ContinueWithOnMainThread(updateTask =>
                {
                    if (updateTask.IsFaulted)
                    {
                        Debug.Log("Error updating DoorUnlocked in Firebase: " + updateTask.Exception);
                    }
                    else
                    {
                        Debug.Log("DoorUnlocked updated successfully in Firebase!");
                    }
                });
            }
        });
    }


}

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
        Player player = new Player(name, email, dateJoined, "default", "", "", 0, 0);

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
                }
            });
        }
    }

    public void StorePlayTime(string uID, int playTime)
    {
        FirebaseDatabase.DefaultInstance.GetReference("players").Child(uID).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task?.Exception);
            }
            else if (task.IsCompleted)
            {
                dbRef.Child("players").Child(uID).Child("TotalPlayTime").SetValueAsync(playTime)
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError("Error storing play time: " + task.Exception);
                    }
                    else if (task.IsCompleted)
                    {
                        Debug.Log("Play time successfully updated to: " + playTime);
                    }
                });
            }
        });
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
    public void UpdateLevelComplete(string uID, string levelName, bool levelCompleted, string achievementID, string dateObtained, bool achievementObtained)
    {
        FirebaseDatabase.DefaultInstance.GetReference("players").Child(uID).Child("Progress").Child(levelName).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                bool doorUnlocked = (bool)snapshot.Child("DoorUnlocked").Value;
                Dictionary<string, object> updateLvl = new Dictionary<string, object>();
                updateLvl[$"Progress/{levelName}/Completed"] = levelCompleted;

                UpdateAchievement(uID, achievementID, dateObtained, achievementObtained);
                FirebaseDatabase.DefaultInstance.GetReference("players")
                .Child(uID) 
                .UpdateChildrenAsync(updateLvl).ContinueWithOnMainThread(updateTask =>
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

    
    public void UpdateAchievement(string uID, string achievementID, string dateObtained, bool achievementObtained)
    {
        FirebaseDatabase.DefaultInstance.GetReference("players").Child(uID).Child("Achievements").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception);
            }
            else if (task.IsCompleted)
            {
                Dictionary<string, object> updateAchievement = new Dictionary<string, object>();
                updateAchievement[$"Achievements/{achievementID}/DateObtained"] = dateObtained;
                updateAchievement[$"Achievements/{achievementID}/Obtained"] = achievementObtained;

                FirebaseDatabase.DefaultInstance.GetReference("players")
                .Child(uID)
                .UpdateChildrenAsync(updateAchievement).ContinueWithOnMainThread(updateTask =>
                {
                    if (updateTask.IsFaulted)
                    {
                        Debug.Log("Error updating Achievements in Firebase: " + updateTask.Exception);
                    }
                    else
                    {
                        Debug.Log("Achievements updated successfully in Firebase!");
                    }
                });
            }
        });
    }

    public void UpdatePlayerPoints(string uID, int points)
    {
        FirebaseDatabase.DefaultInstance.GetReference("players").Child(uID).Child("Points").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception);
            }
            else if (task.IsCompleted)
            {
                dbRef.Child("players").Child(uID).Child("Points").SetValueAsync(points)
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError("Error updating player points: " + task.Exception);
                    }
                    else
                    {
                        Debug.Log("Player points updated successfully to: " + points);
                    }
                });
            }
        });
    }
}

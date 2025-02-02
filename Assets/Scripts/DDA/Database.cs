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

    public void CreateNewPlayer(string uID, string name, string email, string dateJoined, int levelProgress, float totalPlayTime)
    {
        Player player = new Player(name, email, dateJoined, levelProgress, totalPlayTime);
        string playerJson = JsonUtility.ToJson(player);
        Debug.Log(playerJson);
        dbRef.Child("players").Child(uID).SetRawJsonValueAsync(playerJson).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log($"Player {uID} created successfully with account creation timestamp.");
            }
            else
            {
                Debug.LogError($"Failed to create player: {task.Exception}");
            }
        });
    }

    public void ReadPlayerData()
    {

    }
    public void UpdateAchievements()
    {

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using System.Threading.Tasks;
using System;
using TMPro;

public class Authentication : MonoBehaviour
{
    /// <summary>
    /// Firebase Authentication instance for managing user authentication
    /// </summary>
    Firebase.Auth.FirebaseAuth auth;

    public Database database;

    public TextMeshProUGUI emailInput;
    public TextMeshProUGUI passwordInput;
    public TextMeshProUGUI nameInput;

    /// <summary>
    /// Initializes Firebase authentication when the app starts
    /// </summary>
    private void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void SignUp()
    {
        auth.CreateUserWithEmailAndPasswordAsync(emailInput.text, passwordInput.text).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception);
            }
            else if(task.IsCompleted)
            {
                FirebaseUser newPlayer  =  task.Result.User;

                DateTime creationDateTime = DateTimeOffset.FromUnixTimeMilliseconds((long)newPlayer.Metadata.CreationTimestamp).UtcDateTime.ToLocalTime();
                string dateJoined = creationDateTime.ToString("yyyy-MM-dd");

                database.CreateNewPlayer(newPlayer.UserId, nameInput.text, emailInput.text, dateJoined, 0);
                //store data in game manager
                database.ReadPlayerData(newPlayer.UserId);
                database.ReadPlayerLvlProgress(newPlayer.UserId);
                ResetInputs();
                GameManager.Instance.isTracking = true;
                StartCoroutine(GameManager.Instance.TrackPlayTime());
            }
        });
    }

    public void Signout()
    {
        if(auth.CurrentUser != null)
        {
            GameManager.Instance.isTracking = false;
            database.StorePlayTime(GameManager.Instance.playerID, GameManager.Instance.playerPlayTime);
            auth.SignOut();
            GameManager.Instance.StorePlayerDetails("", "", "", "", 0);
        }
    }

    public void Login()
    {
        auth.SignInWithEmailAndPasswordAsync(emailInput.text, passwordInput.text).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception);
            }
            else if (task.IsCompleted)
            {
                AuthResult player = task.Result;
                //store data in game manager
                database.ReadPlayerData(player.User.UserId);
                ResetInputs();
                GameManager.Instance.isTracking = true;
                StartCoroutine(GameManager.Instance.TrackPlayTime());
            }
        });
    }

    private void ResetInputs()
    {
        emailInput.text = "";
        nameInput.text = "";
        passwordInput.text = "";
    }
    public void ResetPassword()
    {
        auth.SendPasswordResetEmailAsync(emailInput.text).ContinueWithOnMainThread(task =>
        {

        });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

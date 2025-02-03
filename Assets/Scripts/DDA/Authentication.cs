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
    [Header("Sign up ui")]
    public TextMeshProUGUI signUpEmailInput;
    public TextMeshProUGUI signUpPasswordInput;
    public TextMeshProUGUI signUpNameInput;

    [Header("Login ui")]
    public TextMeshProUGUI loginEmailInput;
    public TextMeshProUGUI loginPasswordInput;

    [Header("Reset password ui")]
    public TextMeshProUGUI resetPasswordEmailInput;

    /// <summary>
    /// Initializes Firebase authentication when the app starts
    /// </summary>
    private void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void SignUp()
    {
        auth.CreateUserWithEmailAndPasswordAsync(signUpEmailInput.text, signUpPasswordInput.text).ContinueWithOnMainThread(task =>
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

                database.CreateNewPlayer(newPlayer.UserId, signUpNameInput.text, signUpEmailInput.text, dateJoined, 0);
                //store data in game manager
                database.ReadPlayerData(newPlayer.UserId);
                database.ReadPlayerLvlProgress(newPlayer.UserId);
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
        auth.SignInWithEmailAndPasswordAsync(loginEmailInput.text, loginPasswordInput.text).ContinueWithOnMainThread(task =>
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
                database.ReadPlayerLvlProgress(player.User.UserId);
                GameManager.Instance.isTracking = true;
                StartCoroutine(GameManager.Instance.TrackPlayTime());
            }
        });
    }

    private void ResetSignUpInputs()
    {
        signUpEmailInput.text = "";
        signUpNameInput.text = "";
        signUpPasswordInput.text = "";
    }

    private void ResetLoginInputs()
    {
        loginPasswordInput.text = "";
        loginEmailInput.text = "";
    }
    public void ResetPassword()
    {
        auth.SendPasswordResetEmailAsync(resetPasswordEmailInput.text).ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted || task.IsCanceled)
            {
                //error handling
            }
            else if (task.IsCompleted)
            {
                //confirmation text
            }
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

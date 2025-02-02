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
                AuthResult newPlayer = task.Result;
                FirebaseUser newUser   =  task.Result.User;

                DateTime creationDate = DateTimeOffset.FromUnixTimeMilliseconds((long)newUser.Metadata.CreationTimestamp).UtcDateTime;
                string accountCreationDate = creationDate.ToString();
                Debug.Log(accountCreationDate);

                database.CreateNewPlayer(newPlayer.User.UserId, nameInput.text, emailInput.text, accountCreationDate, 0, 0f);
                GameManager.Instance.StorePlayerDetails(newPlayer.User.UserId, nameInput.text, emailInput.text, accountCreationDate);

            }
        });
    }

    public void Signout()
    {
        if(auth.CurrentUser != null)
        {
            auth.SignOut();
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

            }
        });
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supabase;
using System.Threading.Tasks;
using System;
using Supabase.Gotrue;
using TMPro;

public class Authentication : MonoBehaviour
{
    private Supabase.Client supabase;
    public Database database;
    public KioskManager kioskManager;

    [Header("Sign up UI")]
    public TMP_InputField signUpEmailInput;    
    public TMP_InputField signUpPasswordInput; 
    public TMP_InputField signUpNameInput;     
    
    [Header("Log In UI")]
    public TMP_InputField loginEmailInput;    
    public TMP_InputField loginPasswordInput; 

    async void Start()
    {
        database = FindObjectOfType<Database>();

        if (database == null)
        {
            Debug.LogError("Database instance not found in the scene!");
            return;
        }

        supabase = await InitializeSupabase();
        if (supabase == null)
        {
            Debug.LogError("Failed to initialize Supabase!");
        }
    }

    private async Task<Supabase.Client> InitializeSupabase()
    {
        string supabaseUrl = "https://imfbtilewhhhbqtcwjuh.supabase.co";
        string supabaseAnonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImltZmJ0aWxld2hoaGJxdGN3anVoIiwicm9sZSI6ImFub24iLCJpYXQiOjE3Mzc0Njg0NDcsImV4cCI6MjA1MzA0NDQ0N30.Dt_JC_tDi4gtF5yq2pLSk_gk2B8RbIV1hGNcyk0eGFg";
        var clientOptions = new Supabase.SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        };
        
        var client = new Supabase.Client(supabaseUrl, supabaseAnonKey, clientOptions);
        await client.InitializeAsync();
        Debug.Log("Supabase initialized");
        return client;
    }

    public async void SignUp()
    {
        if (supabase == null)
        {
            Debug.LogError("Supabase is not initialized. Cannot sign up.");
            return;
        }

        string email = signUpEmailInput.text.Trim();
        string password = signUpPasswordInput.text.Trim();
        string name = signUpNameInput.text.Trim();

        if (string.IsNullOrEmpty(email) || !email.Contains("@"))
        {
            Debug.LogError("Invalid email format. Please enter a valid email address.");
            return;
        }

        if (database == null)
        {
            Debug.LogError("Database is not initialized!");
            return;
        }

        Debug.Log($"Attempting to sign up with name: {name} and email: {email}");
        try
        {
            var response = await supabase.Auth.SignUp(email, password);
        
            if (response == null || response.User == null)
            {
                Debug.LogError("Sign-up failed: No user returned.");
                return;
            }

            Debug.Log($"Sign up successful for user: {response.User.Id}");
            DateTime creationDateTime = DateTime.UtcNow;
            string dateJoined = creationDateTime.ToString("yyyy-MM-dd");

            database.CreateNewPlayer(response.User.Id, name, email, dateJoined);
            database.StorePlayTime(response.User.Id, 0);
            database.ReadPlayerData(response.User.Id);
            database.ReadPlayerLvlProgress(response.User.Id);
            ResetSignUpInputs();
            //dyslexia unlocked
            kioskManager.DyslexiaButtonUnlock();

            if (GameManager.Instance != null)
            {
                GameManager.Instance.isTracking = true;
                StartCoroutine(GameManager.Instance.TrackPlayTime());
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Sign-up error: {ex.Message}");
            if (ex.InnerException != null)
            {
                Debug.LogError($"Inner exception: {ex.InnerException.Message}");
            }
        }
    }


       
    public async void Signout()
    {
        if (supabase.Auth.CurrentUser != null)
        {
            GameManager.Instance.isTracking = false;
            database.StorePlayTime(GameManager.Instance.playerID, GameManager.Instance.playerPlayTime);
            await supabase.Auth.SignOut();
            GameManager.Instance.StorePlayerDetails("", "", "", "", 0, 0);
            //all buttons locked
        }
    }

    public async void Login()
    {
        try
        {
            var session = await supabase.Auth.SignIn(loginEmailInput.text, loginPasswordInput.text);
            if (session.User != null)
            {
                database.ReadPlayerData(session.User.Id);
                database.ReadPlayerLvlProgress(session.User.Id);
                GameManager.Instance.isTracking = true;
                StartCoroutine(GameManager.Instance.TrackPlayTime());
                ResetLoginInputs();
                //buttons for completed and current level unlocked
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Login error: {ex.Message}");
        }
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
}

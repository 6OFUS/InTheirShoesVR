using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supabase;
using System.Threading.Tasks;
using System;
using Supabase.Gotrue;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class Authentication : MonoBehaviour
{
    private Supabase.Client supabase;
    Database database;
    KioskManager kioskManager;

    public GameObject confirmationPage;
    public GameObject signupPage;
    public GameObject loginPage;

    [Header("Sign up UI")]
    public TMP_InputField signUpEmailInput;    
    public TMP_InputField signUpPasswordInput; 
    public TMP_InputField signUpNameInput;     
    public TextMeshProUGUI signUpError;
    
    [Header("Log In UI")]
    public TMP_InputField loginEmailInput;    
    public TMP_InputField loginPasswordInput; 
    public TextMeshProUGUI loginError;
    
    [Header("Reset Password UI")]
    public TMP_InputField resetPasswordEmailInput;
    public TextMeshProUGUI resetPasswordError;

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

        SceneManager.sceneLoaded += OnSceneLoaded;
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

    private bool IsValidEmail(string email)
    {
        string emailRegex = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
        return Regex.IsMatch(email, emailRegex);
    }

    public async void SignUp()
    {
        if (supabase == null)
        {
            signUpError.text = "Supabase is not initialized.";
            return;
        }

        string email = signUpEmailInput.text.Trim();
        string password = signUpPasswordInput.text.Trim();
        string name = signUpNameInput.text.Trim();

        if (!IsValidEmail(email))
        {
            signUpError.text = "Invalid email format.";
            return;
        }
        if (password.Length < 6)
        {
            signUpError.text = "Password must be at least 6 characters long.";
            return;
        }
        if (string.IsNullOrEmpty(name))
        {
            signUpError.text = "Name cannot be empty.";
            return;
        }

        try
        {
            var response = await supabase.Auth.SignUp(email, password);
            if (response.User == null)
            {
                signUpError.text = "Sign-up failed.";
                return;
            }

            database.CreateNewPlayer(response.User.Id, name, email, DateTime.UtcNow.ToString("yyyy-MM-dd"));
            ResetSignUpInputs();
            confirmationPage.SetActive(true);
            signupPage.SetActive(false);
        }
        catch (Exception ex)
        {
            signUpError.text = ex.Message;
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
                ResetLoginInputs();
                confirmationPage.SetActive(true);
                loginPage.SetActive(false);
            }
            else
            {
                loginError.text = "Invalid email or password.";
            }
        }
        catch (Exception ex)
        {
            loginError.text = ex.Message;
        }
    }

    public async void ResetPassword()
    {
        string email = resetPasswordEmailInput.text.Trim();
        if (!IsValidEmail(email))
        {
            resetPasswordError.text = "Please enter a valid email address.";
            return;
        }

        try
        {
            await supabase.Auth.ResetPasswordForEmail(email);
            resetPasswordError.text = "Password reset email sent!";
        }
        catch (Exception ex)
        {
            resetPasswordError.text = ex.Message;
        }
    }

    public async void Signout()
    {
        if (supabase.Auth.CurrentUser != null)
        {
            await supabase.Auth.SignOut();
            confirmationPage.SetActive(false);
            loginPage.SetActive(true);
        }
    }

    private void ResetSignUpInputs()
    {
        signUpEmailInput.text = "";
        signUpNameInput.text = "";
        signUpPasswordInput.text = "";
        signUpError.text = "";
    }

    private void ResetLoginInputs()
    {
        loginEmailInput.text = "";
        loginPasswordInput.text = "";
        loginError.text = "";
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0) 
        {
            kioskManager = FindObjectOfType<KioskManager>();
        }
    }
}

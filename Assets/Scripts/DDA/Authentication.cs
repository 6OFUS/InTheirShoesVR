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
    MessagesController messagesController;
    public KioskManager kioskManager;
    public TutorialDoor tutorialDoor;

    public GameObject confirmationPage;
    public GameObject signupPage;
    public GameObject loginPage;
    public GameObject completeGameUI;

    public AnimationClip loadingClip;
    [Header("Sign up UI")]
    public TMP_InputField signUpEmailInput;    
    public TMP_InputField signUpPasswordInput; 
    public TMP_InputField signUpNameInput;     
    public TextMeshProUGUI signUpError;
    public GameObject signUpLoadingPage;
    
    [Header("Log In UI")]
    public TMP_InputField loginEmailInput;    
    public TMP_InputField loginPasswordInput; 
    public TextMeshProUGUI loginError;
    public GameObject loginLoadingPage;

    [Header("Reset Password UI")]
    public TMP_InputField resetPasswordEmailInput;
    public TextMeshProUGUI resetPasswordError;
    public GameObject resetPasswordLoadingPage;

    [Header("Sign out")]
    public GameObject signOutLoadingPage;

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

        messagesController = FindObjectOfType<MessagesController>();
    }

    private void Awake()
    {
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

    private IEnumerator LoadingAnim(GameObject loading, AnimationClip clip)
    {
        loading.SetActive(true);
        yield return new WaitForSeconds(clip.length);
        loading.SetActive(false);
    }
    public async void SignUp()
    {
        signUpError.text = "";
        StartCoroutine(LoadingAnim(signUpLoadingPage, loadingClip));

        await Task.Delay(Mathf.RoundToInt(loadingClip.length * 1000));
        if (supabase == null)
        {
            AudioManager.Instance.sfxSource.clip = AudioManager.Instance.loginFailed;
            AudioManager.Instance.sfxSource.Play();
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
        if(signUpError.text != "")
        {
            AudioManager.Instance.sfxSource.clip = AudioManager.Instance.loginFailed;
            AudioManager.Instance.sfxSource.Play();
        }

        try
        {
            var response = await supabase.Auth.SignUp(email, password);
            if (response.User == null)
            {
                signUpError.text = "Sign-up failed.";
                return;
            }
            messagesController.SendNextMessage();
            AudioManager.Instance.sfxSource.clip = AudioManager.Instance.loginSuccess;
            AudioManager.Instance.sfxSource.Play();

            string userID = response.User.Id;
            database.CreateNewPlayer(userID, name, email, DateTime.UtcNow.ToString("yyyy-MM-dd"));
            GameManager.Instance.isTracking = true;
            StartCoroutine(GameManager.Instance.TrackPlayTime());
            database.ReadPlayerData(userID);
            database.ReadPlayerLvlProgress(userID);
            database.UpdateAchievement(userID, "A1", DateTime.UtcNow.ToString("yyyy-MM-dd"), true);
            GameManager.Instance.playerPoints += 10;
            database.UpdatePlayerPoints(userID, GameManager.Instance.playerPoints);

            confirmationPage.SetActive(true);
            signupPage.SetActive(false);
            kioskManager.DyslexiaButtonUnlock();
            tutorialDoor.TutorialUnlocked();
            ResetSignUpInputs();
        }
        catch (Exception ex)
        {
            signUpError.text = ex.Message;
        }
    }

    public async void Login()
    {
        loginError.text = "";
        StartCoroutine(LoadingAnim(loginLoadingPage, loadingClip));
        await Task.Delay(Mathf.RoundToInt(loadingClip.length * 1000));
        try
        {
            var session = await supabase.Auth.SignIn(loginEmailInput.text, loginPasswordInput.text);
            if (session.User != null)
            {
                AudioManager.Instance.sfxSource.clip = AudioManager.Instance.loginSuccess;
                AudioManager.Instance.sfxSource.Play();
                GameManager.Instance.isTracking = true;
                StartCoroutine(GameManager.Instance.TrackPlayTime());
                database.ReadPlayerData(session.User.Id);
                database.ReadPlayerLvlProgress(session.User.Id);
                ResetLoginInputs();
                confirmationPage.SetActive(true);
                loginPage.SetActive(false);
                StartCoroutine(kioskManager.UnlockButtons());
                tutorialDoor.TutorialUnlocked();
            }
            else
            {
                AudioManager.Instance.sfxSource.clip = AudioManager.Instance.loginFailed;
                AudioManager.Instance.sfxSource.Play();
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
        resetPasswordError.text = "";
        string email = resetPasswordEmailInput.text.Trim();
        StartCoroutine(LoadingAnim(resetPasswordLoadingPage, loadingClip));
        await Task.Delay(Mathf.RoundToInt(loadingClip.length * 1000));
        if (!IsValidEmail(email))
        {
            AudioManager.Instance.sfxSource.clip = AudioManager.Instance.loginFailed;
            AudioManager.Instance.sfxSource.Play();
            resetPasswordError.text = "Please enter a valid email address.";
            return;
        }
        try
        {
            await supabase.Auth.ResetPasswordForEmail(email);
            AudioManager.Instance.sfxSource.clip = AudioManager.Instance.loginSuccess;
            AudioManager.Instance.sfxSource.Play();
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
            StartCoroutine(LoadingAnim(signOutLoadingPage, loadingClip));
            await supabase.Auth.SignOut();
            confirmationPage.SetActive(false);
            loginPage.SetActive(true);
            kioskManager.ResetButtons();
            tutorialDoor.TutorialLocked();
            GameManager.Instance.isTracking = false;
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
            tutorialDoor = FindObjectOfType<TutorialDoor>();
            if(GameManager.Instance.playerID != "")
            {
                confirmationPage.SetActive(true);
                loginPage.SetActive(false);
            }
            if (GameManager.Instance.playerLevelProgress["Hearing"].completed)
            {
                completeGameUI.SetActive(true);
                database.UpdateAchievement(GameManager.Instance.playerID, "A6", DateTime.UtcNow.ToString("yyyy-MM-dd"), true);
                database.UpdatePlayerPoints(GameManager.Instance.playerID, GameManager.Instance.playerPoints + 500);
            }
        }
    }
}

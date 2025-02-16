/*
* Author: Alfred Kang
* Date: 15/2/2025
* Description: Skin application onto camera and phone
* */
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Supabase;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class ApplySkins : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of ApplySkins to ensure only one instance exists.
    /// </summary>
    public static ApplySkins Instance;

    /// <summary>
    /// The Renderer component for the phone skin, used to apply the skin texture.
    /// </summary>
    public Renderer phoneSkinRenderer;

    /// <summary>
    /// The Renderer component for the camera skin, used to apply the skin texture.
    /// </summary>
    public Renderer cameraSkinRenderer;
    
    private Client supabaseClient;
    private DatabaseReference dbRef;
    private string phoneSkinFile;
    private string cameraSkinFile;

    /// <summary>
    /// Ensures that only one instance of ApplySkins exists and doesn't get destroyed when loading new scenes.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

    }

    /// <summary>
    /// Initializes Firebase and Supabase services at the start of the application.
    /// </summary>
    private async void Start()
    {
        await InitializeFirebase();
        await InitializeSupabase();
        Debug.Log("App started");
    }

    /// <summary>
    /// Initializes Firebase services and checks dependencies.
    /// </summary>
    private async Task InitializeFirebase()
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            dbRef = FirebaseDatabase.DefaultInstance.RootReference;
            Debug.Log("Firebase initialized.");
        }
        else
        {
            Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
        }
    }

    /// <summary>
    /// Initializes Supabase client and loads the session.
    /// </summary>
    private async Task InitializeSupabase()
    {
        supabaseClient = new Client(
            "https://imfbtilewhhhbqtcwjuh.supabase.co",
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImltZmJ0aWxld2hoaGJxdGN3anVoIiwicm9sZSI6ImFub24iLCJpYXQiOjE3Mzc0Njg0NDcsImV4cCI6MjA1MzA0NDQ0N30.Dt_JC_tDi4gtF5yq2pLSk_gk2B8RbIV1hGNcyk0eGFg"
        );
        supabaseClient.Auth.LoadSession();
        await UpdateSkinTexture(cameraSkinRenderer, cameraSkinFile);
    }

    /// <summary>
    /// Sets up Firebase listeners to monitor changes to the player's skin files.
    /// </summary>
    /// <param name="playerId">The player's unique ID used to fetch data from Firebase.</param>
    public void SetupFirebaseListeners(string playerId)
    {
        Debug.Log("asiopghp");
        if (string.IsNullOrEmpty(playerId))
        {
            Debug.LogError("Player ID is null or empty. Cannot set up listeners.");
            return;
        }
        
        DatabaseReference playerRef = dbRef.Child("players").Child(playerId);
        
        playerRef.Child("PhoneSkinFile").ValueChanged += async (object sender, ValueChangedEventArgs args) =>
        {
            if (args.DatabaseError == null && args.Snapshot.Exists)
            {
                phoneSkinFile = args.Snapshot.Value.ToString();
                await UpdateSkinTexture(phoneSkinRenderer, phoneSkinFile);
            }
        };
        
        playerRef.Child("CameraSkinFile").ValueChanged += async (object sender, ValueChangedEventArgs args) =>
        {
            if (args.DatabaseError == null && args.Snapshot.Exists)
            {
                cameraSkinFile = args.Snapshot.Value.ToString();
                await UpdateSkinTexture(cameraSkinRenderer, cameraSkinFile);
            }
        };
    }

    /// <summary>
    /// Updates the skin texture for the specified renderer using the provided skin file name.
    /// </summary>
    /// <param name="renderer">The renderer component to apply the skin texture to.</param>
    /// <param name="skinFileName">The name of the skin file to apply.</param>
    public async Task UpdateSkinTexture(Renderer renderer, string skinFileName)
    {
        if (renderer == null || string.IsNullOrEmpty(skinFileName))
        {
            Debug.LogError("Renderer is null or skin filename is empty.");
            return;
        }

        string url = $"https://imfbtilewhhhbqtcwjuh.supabase.co/storage/v1/object/public/skins/{skinFileName}";
        Texture2D texture = await GetTextureFromURL(url);

        if (texture != null)
        {
            renderer.material.mainTexture = texture;
            Debug.Log("Texture updated: " + skinFileName);
        }
        else
        {
            Debug.LogError("Failed to download texture: " + skinFileName);
        }
    }

    /// <summary>
    /// Downloads the texture from the specified URL asynchronously.
    /// </summary>
    /// <param name="url">The URL to download the texture from.</param>
    /// <returns>A Texture2D object if successful, or null if the download failed.</returns>
    private async Task<Texture2D> GetTextureFromURL(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            var asyncOperation = request.SendWebRequest();
            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                return DownloadHandlerTexture.GetContent(request);
            }
            else
            {
                Debug.LogError("Error downloading texture: " + request.error);
                return null;
            }
        }
    }
}

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
    public static ApplySkins Instance;
    
    public Renderer phoneSkinRenderer;
    public Renderer cameraSkinRenderer;
    
    private Client supabaseClient;
    private DatabaseReference dbRef;
    private string playerId;
    private string phoneSkinFile;
    private string cameraSkinFile;
    
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

    private async void Start()
    {
        await InitializeFirebase();
        await InitializeSupabase();
        SetupFirebaseListeners();
        Debug.Log("App started");
    }

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

    private async Task InitializeSupabase()
    {
        supabaseClient = new Client(
            "https://imfbtilewhhhbqtcwjuh.supabase.co",
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImltZmJ0aWxld2hoaGJxdGN3anVoIiwicm9sZSI6ImFub24iLCJpYXQiOjE3Mzc0Njg0NDcsImV4cCI6MjA1MzA0NDQ0N30.Dt_JC_tDi4gtF5yq2pLSk_gk2B8RbIV1hGNcyk0eGFg"
        );
        supabaseClient.Auth.LoadSession();
        await UpdateSkinTexture(cameraSkinRenderer, cameraSkinFile);
    }

    public void SetupFirebaseListeners()
    {
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

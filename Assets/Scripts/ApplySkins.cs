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
    // Assign these in the Inspector
    public Renderer phoneSkinRenderer;  // Renderer for the phone skin
    public Renderer cameraSkinRenderer; // Renderer for the camera skin

    private Client supabaseClient;
    
    // These will be populated after login / Firebase fetch
    private string playerId;
    private string PhoneSkinFile;
    private string CameraSkinFile;

    private async void Start()
    {
        // Initialize Firebase (if not already initialized)
        await InitializeFirebase();
        // Initialize Supabase and log in (this example uses debug credentials)
        await InitializeSupabase();
        Debug.Log("App started");
    }

    private async Task InitializeFirebase()
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            Debug.Log("Firebase initialized.");
        }
        else
        {
            Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
        }
    }

    private async Task InitializeSupabase()
    {
        // Initialize your Supabase client (replace with your project details)
        supabaseClient = new Client(
            "https://imfbtilewhhhbqtcwjuh.supabase.co",
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImltZmJ0aWxld2hoaGJxdGN3anVoIiwicm9sZSI6ImFub24iLCJpYXQiOjE3Mzc0Njg0NDcsImV4cCI6MjA1MzA0NDQ0N30.Dt_JC_tDi4gtF5yq2pLSk_gk2B8RbIV1hGNcyk0eGFg"
        );

        // Load any existing session if available
        supabaseClient.Auth.LoadSession();

        // await LoginUser("email", "password");

        // Once logged in, download and apply the textures
        DownloadAndApplyTextures();
    }

    // private async Task LoginUser(string email, string password)
    // {
    //     var response = await supabaseClient.Auth.SignIn(email, password);
    //
    //     if (response != null && response.User != null)
    //     {
    //         // Store the player's ID to be used with Firebase.
    //         playerId = response.User.Id;
    //         Debug.Log("Login successful! Player ID: " + playerId);
    //     }
    //     else
    //     {
    //         Debug.LogError("Login failed. Please check your credentials.");
    //     }
    // }

    // This method fetches the skin file names from Firebase,
    // builds the texture URLs, downloads the textures, and applies them.
    private async void DownloadAndApplyTextures()
    {
        try
        {
            if (string.IsNullOrEmpty(playerId))
            {
                Debug.LogError("Player ID is null or empty. Cannot fetch skin files.");
                return;
            }

            // Fetch skin file names from Firebase at path "players/{playerId}"
            await GetPlayerSkinFiles(playerId);

            // Build the full URLs using the file names from Firebase.
            // (Ensure the file names do not include the ".png" extension if you're appending it here.)
            string phoneSkinURL = $"https://imfbtilewhhhbqtcwjuh.supabase.co/storage/v1/object/public/skins/{PhoneSkinFile}";
            string cameraSkinURL = $"https://imfbtilewhhhbqtcwjuh.supabase.co/storage/v1/object/public/skins/{CameraSkinFile}";

            // Download textures from the URLs.
            Texture2D phoneTexture = await GetTextureFromURL(phoneSkinURL);
            Texture2D cameraTexture = await GetTextureFromURL(cameraSkinURL);

            // Apply the phone texture.
            if (phoneTexture != null)
            {
                if (phoneSkinRenderer != null)
                {
                    phoneSkinRenderer.material.mainTexture = phoneTexture;
                    Debug.Log("Phone texture applied successfully.");
                }
                else
                {
                    Debug.LogError("Phone skin renderer is not assigned.");
                }
            }
            else
            {
                Debug.LogError("Failed to load phone texture.");
            }

            // Apply the camera texture.
            if (cameraTexture != null)
            {
                if (cameraSkinRenderer != null)
                {
                    cameraSkinRenderer.material.mainTexture = cameraTexture;
                    Debug.Log("Camera texture applied successfully.");
                }
                else
                {
                    Debug.LogError("Camera skin renderer is not assigned.");
                }
            }
            else
            {
                Debug.LogError("Failed to load camera texture.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error downloading textures: {e.Message}");
        }
    }

    private async Task GetPlayerSkinFiles(string playerId)
    {
        DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        DataSnapshot snapshot = await dbRef.Child("players").Child(playerId).GetValueAsync();

        if (snapshot.Exists)
        {
            PhoneSkinFile = snapshot.Child("PhoneSkinFile").Value.ToString();
            CameraSkinFile = snapshot.Child("CameraSkinFile").Value.ToString();
            Debug.Log($"Fetched skin files: phone - {PhoneSkinFile}, camera - {CameraSkinFile}");
        }
        else
        {
            Debug.LogError("Player data not found in Firebase database.");
        }
    }

    private async Task<Texture2D> GetTextureFromURL(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            var asyncOperation = request.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                await Task.Yield(); // Wait until download is complete.
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                return DownloadHandlerTexture.GetContent(request);
            }
            else
            {
                Debug.LogError($"Error in UnityWebRequest: {request.error}");
                return null;
            }
        }
    }
}

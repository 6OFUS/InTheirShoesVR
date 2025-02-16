/*
* Author: Alfred Kang
* Date: 15/2/2025
* Description: Gallery script from supabase
* */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Supabase;
using System.Threading.Tasks;
using System.Linq;

public class Gallery : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the Gallery class.
    /// </summary>
    public static Gallery Instance;

    /// <summary>
    /// The parent transform where images will be displayed.
    /// </summary>
    public Transform scrollViewContent;

    /// <summary>
    /// Prefab for displaying images.
    /// </summary>
    public GameObject imagePrefab;

    /// <summary>
    /// Number of images to load per batch.
    /// </summary>
    public int imagesPerBatch = 12;

    /// <summary>
    /// Supabase client for database interactions.
    /// </summary>
    private Client supabaseClient;

    /// <summary>
    /// Tracks the number of images loaded so far.
    /// </summary>
    private int loadedImagesCount = 0;

    /// <summary>
    /// Flag to prevent multiple loading operations at once.
    /// </summary>
    private bool isLoading = false;

    /// <summary>
    /// Ensures there is only one instance of the Gallery script.
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
    /// Initializes Supabase on start.
    /// </summary>
    async void Start()
    {
        await InitializeSupabase();
    }

    /// <summary>
    /// Initializes the Supabase client.
    /// </summary>
    private async Task InitializeSupabase()
    {
        supabaseClient = new Client("https://imfbtilewhhhbqtcwjuh.supabase.co", 
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImltZmJ0aWxld2hoaGJxdGN3anVoIiwicm9sZSI6ImFub24iLCJpYXQiOjE3Mzc0Njg0NDcsImV4cCI6MjA1MzA0NDQ0N30.Dt_JC_tDi4gtF5yq2pLSk_gk2B8RbIV1hGNcyk0eGFg");
        
        supabaseClient.Auth.LoadSession();
    }

    /// <summary>
    /// Loads images from Supabase storage based on the user's ID.
    /// </summary>
    /// <param name="userId">The ID of the user whose images are being loaded.</param>
    public async void LoadImagesFromSupabase(string userId)
    {
        Debug.Log("asiopghp");
        if (isLoading) return;
        isLoading = true;

        if (userId != null)
        {
            string folderPath = $"{userId}/";

            var storage = supabaseClient.Storage.From("playerGalleries");
            var response = await storage.List(folderPath);

            Debug.Log(userId);
            Debug.Log(folderPath);
            if (response != null && response.Count > 0)
            {
                var imagesToLoad = response.Skip(loadedImagesCount).Take(imagesPerBatch);
                foreach (var file in imagesToLoad)
                {
                    string fileUrl = storage.GetPublicUrl(folderPath + file.Name);
                    StartCoroutine(DownloadAndDisplayImage(fileUrl));
                }
                loadedImagesCount += imagesPerBatch;
            }
            else
            {
                Debug.LogError("No images found.");
            }
        }
        else
        {
            
            Debug.LogError("User not authenticated.");
        }
        isLoading = false;
    }

    /// <summary>
    /// Downloads an image from the provided URL and displays it in the UI.
    /// </summary>
    /// <param name="imageUrl">The URL of the image to be downloaded.</param>
    IEnumerator DownloadAndDisplayImage(string imageUrl)
    {
        using (UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((UnityEngine.Networking.DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                GameObject newImageGO = Instantiate(imagePrefab, scrollViewContent);
                Image uiImage = newImageGO.GetComponent<Image>();

                if (uiImage != null)
                {
                    uiImage.sprite = newSprite;
                }
            }
            else
            {
                Debug.LogError("Image download failed: " + request.error);
                GameObject errorImageGO = Instantiate(imagePrefab, scrollViewContent);
                Image errorImage = errorImageGO.GetComponent<Image>();
                if (errorImage != null)
                {
                    errorImage.color = Color.gray; // Placeholder color for failed images
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Supabase;
using System.Threading.Tasks;

public class GallerySlideshow : MonoBehaviour
{
    public Image displayImage; // The Image component where the image will be displayed
    public float slideshowInterval = 5f; // Time interval between image switches
    private Client supabaseClient;
    private bool isLoading = false;
    private string playerId;

    private List<Sprite> loadedImages = new List<Sprite>();
    private int currentImageIndex = 0;

    async void Start()
    {
        await InitializeSupabase();
    }

    private async Task InitializeSupabase()
    {
        supabaseClient = new Client("https://imfbtilewhhhbqtcwjuh.supabase.co", 
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImltZmJ0aWxld2hoaGJxdGN3anVoIiwicm9sZSI6ImFub24iLCJpYXQiOjE3Mzc0Njg0NDcsImV4cCI6MjA1MzA0NDQ0N30.Dt_JC_tDi4gtF5yq2pLSk_gk2B8RbIV1hGNcyk0eGFg");

        supabaseClient.Auth.LoadSession();
        
        await LoginUser("alfredkangjr@gmail.com", "Password123$");
        
        LoadImagesFromSupabase();
    }

    private async Task LoginUser(string email, string password)
    {
        var response = await supabaseClient.Auth.SignIn(email, password);
    
        if (response != null && response.User != null)
        {
            playerId = response.User.Id;
            Debug.Log("Login successful! Player ID: " + playerId);
        }
        else
        {
            Debug.LogError("Login failed. Please check your credentials.");
        }
    }

    async void LoadImagesFromSupabase()
    {
        if (isLoading) return;
        isLoading = true;

        if (supabaseClient.Auth.CurrentUser != null)
        {
            string userId = supabaseClient.Auth.CurrentUser.Id;
            string folderPath = $"{userId}/";

            var storage = supabaseClient.Storage.From("playerGalleries");
            var response = await storage.List(folderPath);

            if (response != null && response.Count > 0)
            {
                foreach (var file in response)
                {
                    string fileUrl = storage.GetPublicUrl(folderPath + file.Name);
                    StartCoroutine(DownloadAndDisplayImage(fileUrl));
                }
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

    IEnumerator DownloadAndDisplayImage(string imageUrl)
    {
        using (UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((UnityEngine.Networking.DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                loadedImages.Add(newSprite);
            }
            else
            {
                Debug.LogError("Image download failed: " + request.error);
            }
        }
    }

    void Update()
    {
        // Start the slideshow when images are loaded
        if (loadedImages.Count > 0 && !isLoading)
        {
            ShowImage(currentImageIndex);

            // Wait for the slideshow interval before switching to the next image
            Invoke("NextImage", slideshowInterval);
        }
    }

    void ShowImage(int index)
    {
        // Set the new image to the display
        displayImage.sprite = loadedImages[index];

        // Preserve the aspect ratio
        displayImage.preserveAspect = true;

        // Adjust the resolution to fit the parent container or canvas (optional)
        ResizeImage(displayImage.sprite);
    }

    void ResizeImage(Sprite sprite)
    {
        // Optionally, you can adjust the size of the image to fit a specific resolution or screen space
        float imageWidth = sprite.rect.width;
        float imageHeight = sprite.rect.height;

        // Set the size of the Image object to fit the sprite's aspect ratio
        float aspectRatio = imageWidth / imageHeight;
        RectTransform rectTransform = displayImage.GetComponent<RectTransform>();

        // Adjust the width or height based on the parent container
        if (rectTransform != null)
        {
            // You can use canvas scaling or adjust the size to fit a target resolution
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.y * aspectRatio, rectTransform.sizeDelta.y);
        }
    }

    void NextImage()
    {
        // Move to the next image in the carousel
        currentImageIndex = (currentImageIndex + 1) % loadedImages.Count;
    }
}

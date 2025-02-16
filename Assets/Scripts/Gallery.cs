using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Supabase;
using System.Threading.Tasks;
using System.Linq;

public class Gallery : MonoBehaviour
{
    public static Gallery Instance;
    public Transform scrollViewContent;
    public GameObject imagePrefab;
    public int imagesPerBatch = 12;

    private Client supabaseClient;
    private int loadedImagesCount = 0;
    private bool isLoading = false;

    private string playerId;

    async void Start()
    {
        await InitializeSupabase();
    }

    private async Task InitializeSupabase()
    {
        supabaseClient = new Client("https://imfbtilewhhhbqtcwjuh.supabase.co", 
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImltZmJ0aWxld2hoaGJxdGN3anVoIiwicm9sZSI6ImFub24iLCJpYXQiOjE3Mzc0Njg0NDcsImV4cCI6MjA1MzA0NDQ0N30.Dt_JC_tDi4gtF5yq2pLSk_gk2B8RbIV1hGNcyk0eGFg");
        
        supabaseClient.Auth.LoadSession();
        
        LoadImagesFromSupabase();
    }

    public async void LoadImagesFromSupabase()
    {
        if (isLoading) return;
        isLoading = true;

        if (supabaseClient.Auth.CurrentUser != null)
        {
            string userId = supabaseClient.Auth.CurrentUser.Id;
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

    void Update()
    {
        if (scrollViewContent.childCount > 0 && scrollViewContent.GetChild(scrollViewContent.childCount - 1).position.y < Screen.height)
        {
            LoadImagesFromSupabase(); // Load next batch when user scrolls down
        }
    }
}

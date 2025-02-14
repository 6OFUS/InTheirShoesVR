using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Supabase;
using UnityEngine.Networking;

public class Polaroid : MonoBehaviour
{
    public GameObject photoPrefab = null;
    public MeshRenderer screenRenderer = null;
    public Transform spawnLocation = null;

    private Camera renderCamera = null;
    public LayerMask subjectLayer;

    private const string SUPABASE_URL = "https://imfbtilewhhhbqtcwjuh.supabase.co";
    private const string SUPABASE_BUCKET = "playerGalleries";
    private const string SUPABASE_API_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImltZmJ0aWxld2hoaGJxdGN3anVoIiwicm9sZSI6ImFub24iLCJpYXQiOjE3Mzc0Njg0NDcsImV4cCI6MjA1MzA0NDQ0N30.Dt_JC_tDi4gtF5yq2pLSk_gk2B8RbIV1hGNcyk0eGFg";
    private string userUID;

    private Client supabaseClient;
    
    private void Awake()
    {
        renderCamera = GetComponentInChildren<Camera>();
    }
    
    private async System.Threading.Tasks.Task InitializeSupabase()
    {
        supabaseClient = new Client(SUPABASE_URL, SUPABASE_API_KEY);
        await supabaseClient.InitializeAsync();

        var session = supabaseClient.Auth.CurrentSession;
        if (session != null && session.User != null)
        {
            userUID = session.User.Id;
            Debug.Log("User UID: " + userUID);
        }
        else
        {
            Debug.LogError("No user logged in!");
        }
    }

    private void Start()
    {
        CreateRenderTexture();
        TurnOff();
    }

    private void CreateRenderTexture()
    {
        RenderTexture newTexture = new RenderTexture(256, 256, 32, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB);
        newTexture.antiAliasing = 4;

        renderCamera.targetTexture = newTexture;
        screenRenderer.material.mainTexture = newTexture;
    }

    public void TakePhoto()
    {
        Photo newPhoto = CreatePhoto();
        Texture2D newTexture = RenderCameraToTexture(renderCamera);
        newPhoto.SetImage(newTexture);
        StartCoroutine(UploadPhotoToSupabase(newTexture));
        
        // Perform a raycast the size of the photo
        CheckForCameraSubjects();
    }
    
    private void CheckForCameraSubjects()
    {
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        
        for (int x = 0; x < screenWidth; x += screenWidth / 10) // Reduce number of checks
        {
            for (int y = 0; y < screenHeight; y += screenHeight / 10)
            {
                Ray ray = renderCamera.ScreenPointToRay(new Vector3(x, y, 0));
                if (Physics.Raycast(ray, out RaycastHit hit, renderCamera.farClipPlane, subjectLayer))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    CameraSubject subject = hit.collider.GetComponent<CameraSubject>();
                    if (subject != null)
                    {
                        subject.Snapped(); // Call Snapped on the detected CameraSubject
                    }
                }
            }
        }
    }

    private Photo CreatePhoto()
    {
        GameObject photoObject = Instantiate(photoPrefab, spawnLocation.position, spawnLocation.rotation, transform);
        return photoObject.GetComponent<Photo>();
    }

    private Texture2D RenderCameraToTexture(Camera camera)
    {
        camera.Render();
        RenderTexture.active = camera.targetTexture;

        Texture2D photo = new Texture2D(768, 256, TextureFormat.RGB24, false);
        photo.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        photo.Apply();

        return photo;
    }

    private IEnumerator UploadPhotoToSupabase(Texture2D texture)
    {
        //TEST: REMOVE ON PROD
        userUID = "e5cec99f-4294-4a61-841f-0361bcc46a45"; // my account's uid
        
        byte[] imageBytes = texture.EncodeToPNG();
        string fileName = $"photo_{DateTime.UtcNow:yyyyMMdd_HHmmss}.png";
        string filePath = $"{userUID}/{fileName}";
        string url = $"{SUPABASE_URL}/storage/v1/object/{SUPABASE_BUCKET}/{filePath}";

        UnityWebRequest request = UnityWebRequest.Put(url, imageBytes);
        request.method = UnityWebRequest.kHttpVerbPUT;
        request.SetRequestHeader("Authorization", $"Bearer {SUPABASE_API_KEY}");
        request.SetRequestHeader("Content-Type", "image/png");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Photo uploaded successfully: " + url);
        }
        else
        {
            Debug.LogError("Error uploading photo: " + request.error);
        }
    }

    public void TurnOn()
    {
        renderCamera.enabled = true;
        screenRenderer.material.color = Color.white;
    }

    public void TurnOff()
    {
        renderCamera.enabled = false;
        screenRenderer.material.color = Color.black;
    }
}
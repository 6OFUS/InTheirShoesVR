/*
* Author: Alfred Kang
* Date: 15/2/2025
* Description: Polaroid camera linking to supabase API
* */
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
    /// <summary>
    /// Prefab for the photo object that will be instantiated when a photo is taken.
    /// </summary>
    public GameObject photoPrefab = null;

    /// <summary>
    /// The MeshRenderer that displays the camera screen.
    /// </summary>
    public MeshRenderer screenRenderer = null;

    /// <summary>
    /// The location where photos will be spawned.
    /// </summary>
    public Transform spawnLocation = null;

    /// <summary>
    /// Camera used for rendering the photo.
    /// </summary>
    private Camera renderCamera = null;

    /// <summary>
    /// Layer mask to identify valid camera subjects.
    /// </summary>
    public LayerMask subjectLayer;

    private const string SUPABASE_URL = "https://imfbtilewhhhbqtcwjuh.supabase.co";
    private const string SUPABASE_BUCKET = "playerGalleries";
    private const string SUPABASE_API_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImltZmJ0aWxld2hoaGJxdGN3anVoIiwicm9sZSI6ImFub24iLCJpYXQiOjE3Mzc0Njg0NDcsImV4cCI6MjA1MzA0NDQ0N30.Dt_JC_tDi4gtF5yq2pLSk_gk2B8RbIV1hGNcyk0eGFg";

    /// <summary>
    /// Unique identifier for the user.
    /// </summary>
    private string userUID;

    private Client supabaseClient;
    private AudioSource audioSource;

    /// <summary>
    /// Initializes the camera and other components, and sets up the Supabase client.
    /// </summary>
    private void Awake()
    {
        renderCamera = GetComponentInChildren<Camera>();
    }

    /// <summary>
    /// Initializes Supabase client and gets the user's session.
    /// </summary>
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

    /// <summary>
    /// Initializes the render texture and sets the camera to off.
    /// </summary>
    private void Start()
    {
        CreateRenderTexture();
        TurnOff();
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Creates the render texture for the camera to render into.
    /// </summary>
    private void CreateRenderTexture()
    {
        RenderTexture newTexture = new RenderTexture(256, 256, 32, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB);
        newTexture.antiAliasing = 4;

        renderCamera.targetTexture = newTexture;
        screenRenderer.material.mainTexture = newTexture;
    }

    /// <summary>
    /// Takes a photo by rendering the camera and uploading the image to Supabase.
    /// </summary>
    public void TakePhoto()
    {
        audioSource.Play();
        Photo newPhoto = CreatePhoto();
        Texture2D newTexture = RenderCameraToTexture(renderCamera);
        newPhoto.SetImage(newTexture);
        StartCoroutine(UploadPhotoToSupabase(newTexture));
        
        // Perform a raycast the size of the photo
        CheckForCameraSubjects();
    }

    /// <summary>
    /// Performs a raycast to detect subjects in the camera's view.
    /// </summary>
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

    /// <summary>
    /// Creates and spawns a new photo object.
    /// </summary>
    /// <returns>A Photo object representing the spawned photo.</returns>
    private Photo CreatePhoto()
    {
        GameObject photoObject = Instantiate(photoPrefab, spawnLocation.position, spawnLocation.rotation, transform);
        return photoObject.GetComponent<Photo>();
    }

    /// <summary>
    /// Renders the camera's view into a texture and returns the resulting photo as a Texture2D.
    /// </summary>
    /// <param name="camera">The camera to render from.</param>
    /// <returns>A Texture2D containing the rendered photo.</returns>
    private Texture2D RenderCameraToTexture(Camera camera)
    {
        camera.Render();
        RenderTexture.active = camera.targetTexture;

        // Get the width and height of the RenderTexture
        int width = camera.targetTexture.width;
        int height = camera.targetTexture.height;

        // Create a Texture2D with the same dimensions as the RenderTexture
        Texture2D photo = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Read the pixels from the RenderTexture into the Texture2D
        photo.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        photo.Apply();

        RenderTexture.active = null;

        return photo;
    }

    /// <summary>
    /// Uploads the captured photo to Supabase storage.
    /// </summary>
    /// <param name="texture">The texture to be uploaded.</param>
    /// <returns>An IEnumerator for Unity's coroutine system.</returns>
    private IEnumerator UploadPhotoToSupabase(Texture2D texture)
    {
        // //TEST: REMOVE ON PROD
        // userUID = "e5cec99f-4294-4a61-841f-0361bcc46a45"; // my account's uid
        //
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

    /// <summary>
    /// Turns on the camera and enables the screen renderer.
    /// </summary>
    public void TurnOn()
    {
        renderCamera.enabled = true;
        screenRenderer.material.color = Color.white;
    }

    /// <summary>
    /// Turns off the camera and disables the screen renderer.
    /// </summary>
    public void TurnOff()
    {
        renderCamera.enabled = false;
        screenRenderer.material.color = Color.black;
    }
}
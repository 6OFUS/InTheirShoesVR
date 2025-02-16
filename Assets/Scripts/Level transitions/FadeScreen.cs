/*
    Author: https://youtu.be/JCyJ26cIM0Y?si=o01lcILk6Gi4gGqv
    Date: 8/2/2025
    Description: The FadeScreen class handles the functions to fade screen for scene transitions
*/
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FadeScreen : MonoBehaviour
{
    /// <summary>
    /// Boolean to start the fade transition when load into scene
    /// </summary>
    public bool fadeOnStart = true;
    /// <summary>
    /// Time taken for fade transitions
    /// </summary>
    public float fadeDuration;
    /// <summary>
    /// Colour of fade transition
    /// </summary>
    public Color fadeColor;
    /// <summary>
    /// Renderer on object used for fade
    /// </summary>
    private Renderer rend;

    /// <summary>
    /// Fades into scene by changing opacity from opaque to transparent
    /// </summary>
    public void FadeIn()
    {
        Fade(1, 0);
    }
    /// <summary>
    /// Fades out of scene by changing opacity from transparent to opaque
    /// </summary>
    public void FadeOut()
    {
        Fade(0,1);
    }
    /// <summary>
    /// Function for fade transition
    /// </summary>
    /// <param name="alphaIn">Initial alpha value</param>
    /// <param name="alphaOut">End alpha value</param>
    public void Fade(float alphaIn, float alphaOut)
    {
        this.gameObject.SetActive(true);
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }
    /// <summary>
    /// Coroutine for fade transition
    /// </summary>
    /// <param name="alphaIn">Initial alpha value</param>
    /// <param name="alphaOut">End alpha value</param>
    /// <returns></returns>
    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0;
        while (timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer/ fadeDuration);
            rend.material.SetColor("_Color",newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        fadeColor.a = alphaOut;
        rend.material.SetColor("_Color", fadeColor);
        this.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        if (fadeOnStart)
        {
            FadeIn();
        }
    }
}

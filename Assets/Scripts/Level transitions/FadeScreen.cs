/*
    Author: https://youtu.be/JCyJ26cIM0Y?si=o01lcILk6Gi4gGqv
    Date: 30/1/2025
    Description: The 
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
    public bool fadeOnStart = true;
    public float fadeDuration;
    public Color fadeColor;
    private Renderer rend;


    public void FadeIn()
    {
        Fade(1, 0);
    }

    public void FadeOut()
    {
        Fade(0,1);
    }
    public void Fade(float alphaIn, float alphaOut)
    {
        this.gameObject.SetActive(true);
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }
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

    // Update is called once per frame
    void Update()
    {

    }
}

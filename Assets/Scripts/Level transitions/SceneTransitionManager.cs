/*
    Author: https://youtu.be/JCyJ26cIM0Y?si=o01lcILk6Gi4gGqv
    Date: 8/2/2025
    Description: The SceneTransitionManager class 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public FadeScreen fadeScreen;

    public void ChangeSceneAsyc(int sceneIndex)
    {
        StartCoroutine(ChangeSceneAsycRoutine(sceneIndex));
    }

    IEnumerator ChangeSceneAsycRoutine(int sceneIndex)
    {
        fadeScreen.FadeOut();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        float timer = 0f;
        while(timer <= fadeScreen.fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
        
    }
}

/*
    Author: Kevin Heng
    Date: 30/1/2025
    Description: The SceneChanger class is used to handle the functions for scene changes
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public int sceneIndex;

    public Animator transition;
    public float transitionTime = 1f;

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    public void LoadScene()
    {
        transition.gameObject.SetActive(true);
        StartCoroutine(LoadLevel(sceneIndex));
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

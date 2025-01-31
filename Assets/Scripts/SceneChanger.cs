/*
    Author: Kevin Heng
    Date: 30/1/2025
    Description: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public int sceneIndex;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneIndex);
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

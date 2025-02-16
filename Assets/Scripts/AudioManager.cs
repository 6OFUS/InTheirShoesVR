/*
* Author: Jennifer Lau
* Date: 15/2/2025
* Description: Audio Manager
* */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the AudioManager.
    /// </summary>
    public static AudioManager Instance;
    
    [Header("Audio sources")]
    /// <summary>
    /// The audio source used for playing sound effects.
    /// </summary>
    public AudioSource sfxSource;
    [Header("Audio clips")]
    /// <summary>
    /// Audio clip played when login is successful.
    /// </summary>
    public AudioClip loginSuccess;
    /// <summary>
    /// Audio clip played when login fails.
    /// </summary>
    public AudioClip loginFailed;

    /// <summary>
    /// Ensures only one instance of AudioManager exists and persists between scenes.
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
}

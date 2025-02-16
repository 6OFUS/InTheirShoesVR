/*
    Author: Kevin Heng
    Date: 7/2/2025
    Description: The KeycardScanner class is used to handle the function of scanning keycard to open door
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycardScanner : MonoBehaviour
{
    /// <summary>
    /// Keycard tag
    /// </summary>
    public string keycardTag;
    /// <summary>
    /// Boolean for when correct keycard is scanned
    /// </summary>
    public bool keycardScanned;
    /// <summary>
    /// Reference Databse class
    /// </summary>
    Database database;
    /// <summary>
    /// Audio source for when object is scanned
    /// </summary>
    
    [Header("Audio")]
    public AudioSource scanAudioSource;
    /// <summary>
    /// Array of audio clips to play when object is scanned
    /// </summary>
    public AudioClip[] scanAudioClips;

    /// <summary>
    /// Door locked icon above door
    /// </summary>
    [Header("UI")]
    public GameObject doorLockedIcon;
    /// <summary>
    /// Door unlocked icon above door
    /// </summary>
    public GameObject doorUnlockedIcon;

    /// <summary>
    /// Trigger area to scan object
    /// </summary>
    /// <param name="other">Object to be scanned</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(keycardTag))
        {
            doorLockedIcon.SetActive(false);
            doorUnlockedIcon.SetActive(true);
            keycardScanned = true;
            Debug.Log("Scanned");
            scanAudioSource.clip = scanAudioClips[0];
            scanAudioSource.Play();
            database.UpdateDoorLockStatus(GameManager.Instance.playerID, keycardTag, keycardScanned);
        }
        else
        {
            scanAudioSource.clip = scanAudioClips[1];
            scanAudioSource.Play();
            Debug.Log("This is not the right keycard");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        database = FindObjectOfType<Database>();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilityEndPoint : MonoBehaviour
{
    Database database;
    public string currentLevelName;
    private AudioSource audioSource;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            database.UpdateLevelComplete(GameManager.Instance.playerID, currentLevelName, true, "A4", DateTime.UtcNow.ToString("yyyy-MM-dd"), true);
            audioSource.Play();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        database = FindObjectOfType<Database>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

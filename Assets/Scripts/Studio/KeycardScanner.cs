using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycardScanner : MonoBehaviour
{
    public string keycardTag;
    public bool keycardScanned;
    Database database;
    public AudioSource scanAudioSource;
    public AudioClip[] scanAudioClips;
    public GameObject doorLockedIcon;
    public GameObject doorUnlockedIcon;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}

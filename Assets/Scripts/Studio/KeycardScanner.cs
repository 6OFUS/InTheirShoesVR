using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycardScanner : MonoBehaviour
{
    public string keycardTag;
    public bool keycardScanned;
    Database database;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(keycardTag))
        {
            keycardScanned = true;
            Debug.Log("Scanned");
            database.UpdateDoorLockStatus(GameManager.Instance.playerID, keycardTag, keycardScanned);
        }
        else
        {
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

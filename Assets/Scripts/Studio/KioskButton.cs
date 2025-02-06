using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KioskButton : MonoBehaviour
{
    public GameObject keycard;
    public GameObject spawnPoint;

    public void KeycardDispense()
    {
        Instantiate(keycard, spawnPoint.transform.position, spawnPoint.transform.rotation);
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

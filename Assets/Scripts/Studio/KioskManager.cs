using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KioskManager : MonoBehaviour
{
    public GameObject dyslexiaLocked;
    public GameObject dyslexiaUnlocked;
    public void DyslexiaButtonUnlock()
    {
        dyslexiaLocked.SetActive(false);
        dyslexiaUnlocked.SetActive(true);
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

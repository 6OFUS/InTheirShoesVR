/*
    Author: Kevin Heng
    Date: 30/1/2025
    Description: The EndPoint class is used to handle the functions when player reaches end point of level
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public GameObject endUI;

    private void OnTriggerEnter(Collider other)
    {
        endUI.SetActive(true);
        Debug.Log("Level completed");
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

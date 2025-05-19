using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoSound : MonoBehaviour
{
    private Customer customer;
    public MessagesController messagesController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            Debug.Log("enter cafe");
            messagesController.SendMultipleMessages(3,2);
            AudioListener.volume = 0.025f;
            customer.WalkToCounter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioListener.volume = 1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        customer = FindObjectOfType<Customer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

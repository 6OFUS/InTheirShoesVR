/*
    Author: Kevin Heng
    Date: 27/1/2025
    Description: The VehicleMovement class is used to handle the vehicle movement with the traffic light system
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    TrafficLightController trafficLightController;
    private void OnTriggerEnter(Collider other)
    {
        if (!trafficLightController.greenManBlinking && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("hit car");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        trafficLightController = FindObjectOfType<TrafficLightController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

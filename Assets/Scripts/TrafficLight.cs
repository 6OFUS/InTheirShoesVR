/*
    Author: Kevin Heng
    Date: 27/1/2025
    Description: The TrafficLight class is used to handle the vehicle traffic lights for player to cross the road
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public GameObject greenLight;
    public GameObject amberLight;
    public GameObject redLight;

    TrafficLightController trafficLightController;

    public void ChangeLight(string currentState)
    {
        switch (currentState)
        {
            case "green":
                greenLight.SetActive(true);
                amberLight.SetActive(false);
                redLight.SetActive(false);
                break;
            case "amber":
                greenLight.SetActive(false);
                amberLight.SetActive(true);
                redLight.SetActive(false);
                break;
            case "red":
                greenLight.SetActive(false);
                amberLight.SetActive(false);
                redLight.SetActive(true);
                break;
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

/*
    Author: Kevin Heng
    Date: 27/1/2025
    Description: The Pedestrian class is used to handle the traffic light system for player to cross the road
*/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PedestrianTrafficLight : MonoBehaviour
{
    TrafficLightController trafficLightController;

    public GameObject redMan;
    public GameObject greenMan;

    public TextMeshProUGUI crossingTimerText;

    public void PressButton()
    {
        if (trafficLightController.buttonPressed)
        {
            Debug.Log("button pressed already");
            return;
        }
        else
        {
            trafficLightController.buttonPressed = true;
            Debug.Log("button pressed");
            trafficLightController.Crossing();
        }
    }

    public void ChangeCrossingLight(string currentState)
    {
        switch (currentState)
        {
            case "red":
                redMan.SetActive(true);
                greenMan.SetActive(false);
                currentState = "green";
                break;
            case "green":
                greenMan.SetActive(true);
                redMan.SetActive(false);
                currentState = "red";
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

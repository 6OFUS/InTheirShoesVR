/*
    Author: Hui Hui
    Date: 27/1/2025
    Description: The TrafficLight class is used to handle the vehicle traffic lights for player to cross the road
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    [Header("Traffic light colours")]
    /// <summary>
    /// Green light
    /// </summary>
    public GameObject greenLight;
    /// <summary>
    /// Amber light
    /// </summary>
    public GameObject amberLight;
    /// <summary>
    /// Red light
    /// </summary>
    public GameObject redLight;

    /// <summary>
    /// Reference the TrafficLightController script
    /// </summary>
    private TrafficLightController trafficLightController;

    /// <summary>
    /// Controls state transitions of a vehicle traffic light using a FSM
    /// Updates when the traffic light colour changes
    /// </summary>
    /// <param name="currentState">Current traffic light colour</param>
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

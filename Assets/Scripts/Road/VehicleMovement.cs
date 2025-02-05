/*
    Author: Kevin Heng
    Date: 28/1/2025
    Description: The VehicleMovement class is used to handle the vehicle movement with the traffic light system
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : SceneChanger
{
    /// <summary>
    /// Reference the TrafficLightController script
    /// </summary>
    private TrafficLightController trafficLightController;

    [Header("Driving markers")]
    /// <summary>
    /// Point where vehicle starts driving
    /// </summary>
    public GameObject startMarker;
    /// <summary>
    /// Position of crossing line
    /// </summary>
    public GameObject crossingLine;
    /// <summary>
    /// Point where vehicle stops and returns to start point
    /// </summary>
    public GameObject endMarker;
    [Header("Vehicle properties")]
    /// <summary>
    /// Current speed of vehicle
    /// </summary>
    public float currentSpeed = 0f;
    /// <summary>
    /// Max speed of vehicle
    /// </summary>
    public float maxSpeed;
    /// <summary>
    /// Distance from stopMarker before vehicle slows down and stops
    /// </summary>
    public float slowDownDistance;
    /// <summary>
    /// Minimum distance from stopMarker for vehicle to either stop or continue moving
    /// </summary>
    public float distanceThreshold;
    /// <summary>
    /// Time to accelerate to max speed
    /// </summary>
    public float accelerationTime;
    /// <summary>
    /// Time taken for the acceleration
    /// </summary>
    private float accelerationTimer;

    [Header("Car detection")]
    /// <summary>
    /// Layer for detecting other vehicles
    /// </summary>
    public LayerMask vehicleLayer;
    /// <summary>
    /// Distance to check for a car ahead
    /// </summary>
    public float carDetectionRange = 5f;


    /// <summary>
    /// Checks if there is a car in front
    /// </summary>
    /// <returns>True if a car is detected in front, otherwise false</returns>
    private bool IsCarInFront()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, carDetectionRange, vehicleLayer))
        {
            return true;
        }
        return false;

    }
    /// <summary>
    /// Handles the vehicle driving mechanic
    /// </summary>
    private void Drive()
    {
        float distanceToStoppingPoint = Vector3.Distance(transform.position, crossingLine.transform.position);
        bool carAhead = IsCarInFront();  // Check if there is a car in front

        // Check if the car is at a red or amber light
        switch (trafficLightController.trafficLightsCurrentState)
        {
            case "green":
                if (!carAhead)
                {
                    // Handle acceleration if there is no car ahead
                    if (currentSpeed == 0)  // Reset only when stopped
                    {
                        accelerationTimer = 0f;
                    }

                    if (accelerationTimer < accelerationTime)
                    {
                        accelerationTimer += Time.deltaTime;
                        float t = Mathf.InverseLerp(0, accelerationTime, accelerationTimer);
                        currentSpeed = Mathf.Lerp(0, maxSpeed, t);
                    }
                    else
                    {
                        currentSpeed = maxSpeed;
                    }
                }
                else
                {
                    // Reduce speed when a car is ahead
                    currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed * 0.5f, Time.deltaTime * 2f);
                }
                break;

            case "amber":
            case "red":
                // Apply deceleration logic for both amber and red lights
                if (carAhead)
                {
                    // Car ahead is detected, so we apply deceleration based on distance to the car in front
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.forward, out hit, carDetectionRange, vehicleLayer))
                    {
                        float distanceToCarAhead = hit.distance;

                        // If the car is too close, slow down more aggressively
                        if ((distanceToCarAhead < slowDownDistance && distanceToCarAhead > 0) || distanceToCarAhead < distanceThreshold)
                        {
                            float t = Mathf.InverseLerp(slowDownDistance, 0.5f, distanceToCarAhead);
                            currentSpeed = Mathf.Lerp(maxSpeed, 0, t);
                        }
                    }
                }
                else
                {
                    // Normal deceleration if no car ahead
                    if (distanceToStoppingPoint < slowDownDistance && distanceToStoppingPoint > 0)
                    {
                        float t = Mathf.InverseLerp(slowDownDistance, 0, distanceToStoppingPoint);
                        currentSpeed = Mathf.Lerp(maxSpeed, 0, t);
                    }
                    else if (distanceToStoppingPoint <= distanceThreshold)
                    {
                        currentSpeed = 0;
                    }
                }
                break;
        }

        // Move the car
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        // Reset car to start if it reaches the end marker
        float distanceToEndPoint = Vector3.Distance(transform.position, endMarker.transform.position);
        if (distanceToEndPoint <= distanceThreshold)
        {
            transform.position = startMarker.transform.position;
        }
    }


    /// <summary>
    /// When vehicle hits player
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && !trafficLightController.canCross)
        {
            LoadScene();
            Debug.Log("player hit");
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        trafficLightController = FindObjectOfType<TrafficLightController>();
        transform.position = startMarker.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        Drive();
    }
}

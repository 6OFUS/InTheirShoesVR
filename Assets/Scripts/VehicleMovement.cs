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

    public GameObject startMarker;
    public GameObject stopMarker;
    public GameObject endMarker;

    public float currentSpeed = 0f;
    public float originalSpeed;
    public float slowDownDistance;
    public float distanceThreshold;

    public float accelerationTime;
    float accelerationTimer;

    void Drive()
    {
        Vector3 stoppingPoint = stopMarker.transform.position;
        float distanceToStoppingPoint = Vector3.Distance(transform.position, stoppingPoint);
        switch (trafficLightController.trafficLightsCurrentState)
        {
            case "green":
                if (currentSpeed == 0)  // Reset only when stopped
                {
                    accelerationTimer = 0f;
                }

                if (accelerationTimer < accelerationTime)
                {
                    accelerationTimer += Time.deltaTime;
                    float t = Mathf.InverseLerp(0, accelerationTime, accelerationTimer);
                    currentSpeed = Mathf.Lerp(0, originalSpeed, t);
                }
                break;
            case "amber":
            case "red":
                if (distanceToStoppingPoint < slowDownDistance && distanceToStoppingPoint > 0)
                {
                    float t = Mathf.InverseLerp(slowDownDistance, 0, distanceToStoppingPoint);
                    currentSpeed = Mathf.Lerp(originalSpeed, 0, t);
                }

                if (distanceToStoppingPoint <= distanceThreshold)
                {
                    currentSpeed = 0;
                }
                break;
        }
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        //reset car to start
        float distanceToEndPoint = Vector3.Distance(transform.position, endMarker.transform.position);
        if (distanceToEndPoint <= distanceThreshold)
        {
            transform.position = startMarker.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && !trafficLightController.canCross)
        {
            //restart ui appear
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

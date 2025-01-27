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
    public PedestrianTrafficLight pedestrianTrafficLight;

    public void Move() //put in Update function
    {
        if (!pedestrianTrafficLight.canCross) //canCross = false
        {
            //vehicle move
        }
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

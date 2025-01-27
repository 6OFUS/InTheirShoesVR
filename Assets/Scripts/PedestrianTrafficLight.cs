/*
    Author: Kevin Heng
    Date: 27/1/2025
    Description: The Pedestrian class is used to handle the traffic light system for player to cross the road
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianTrafficLight : MonoBehaviour
{
    /// <summary>
    /// Boolean to set to true when player presses traffic light button
    /// </summary>
    bool buttonPressed;

    /// <summary>
    /// Boolean to set to true if player can cross the road
    /// </summary>
    public bool canCross;

    public void PressButton()
    {
        if (buttonPressed)
        {
            Debug.Log("button pressed already");
            return;
        }
        else
        {
            buttonPressed = true;
            Debug.Log(buttonPressed);
            ChangeLight();
        }
    }

    public void ChangeLight()
    {
        if (buttonPressed)
        {
            //wait a few seconds
            //traffic light change from green to amber to red
            canCross = true;
            if (canCross)
            {
                //change to green man and show timer text
                //when timer = 0
                canCross = false;
                //change to red man and traffic light change to green
            }

        }
        else
        {
            //wait a few seconds
            //then
            //traffic light change from red to green
            //then
            //change to red man
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

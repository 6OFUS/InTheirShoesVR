/*
    Author: Kevin Heng
    Date: 27/1/2025
    Description: The PedestrianTrafficLight class is used to handle the pedestrian traffic lights for player to cross the road
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
    public GameObject buttonLight;

    public TextMeshProUGUI crossingTimerText;

    public AudioSource beepingAudioSource;
    public AudioClip[] beepingAudioClips;
    public int clipIndex;
    public AudioSource pressButton;

    public void PressButton()
    {
        pressButton.Play();
        if (trafficLightController.buttonPressed)
        {
            Debug.Log("button pressed already");
            return;
        }
        else
        {
            trafficLightController.buttonPressed = true;
            Debug.Log("button pressed");
            StartCoroutine(BeforeBeeping());
        }
    }

    IEnumerator BeforeBeeping()
    {
        beepingAudioSource.clip = beepingAudioClips[clipIndex];
        yield return new WaitForSeconds(1);
        beepingAudioSource.Play();
        trafficLightController.Crossing();
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
                buttonLight.SetActive(false);
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
        buttonLight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

/*
    Author: Kevin Heng
    Date: 27/1/2025
    Description: The TrafficLightController class is used to handle the traffic light system of all traffic lights (vehicle and pedestrian lights)
*/
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    [Header("Traffic lights")]
    /// <summary>
    /// List of all vehicle traffic lights
    /// </summary>
    public List<TrafficLight> trafficLights;
    /// <summary>
    /// List of all pedestrian crossing lights
    /// </summary>
    public List<PedestrianTrafficLight> pedestrianTrafficLights;

    [Header("Traffic light states")]
    /// <summary>
    /// To control the current state of all vehicle traffic lights
    /// </summary>
    public string trafficLightsCurrentState = "green";
    /// <summary>
    /// To control the current state of all pedestrian crossing lights
    /// </summary>
    public string pedestrianLightsCurrentState = "red";

    [Header("Pedestrian crossing")]
    /// <summary>
    /// To check if traffic light button is pressed by player
    /// </summary>
    public bool buttonPressed;
    /// <summary>
    /// Time to wait until player can cross the road
    /// </summary>
    public int beforeLightChangeTime;
    /// <summary>
    /// Time interval for vehicle traffic light to change from one colour to the next
    /// </summary>
    public int lightChangeTime;
    /// <summary>
    /// Time given for player to cross the road
    /// </summary>
    public int crossingTime;
    /// <summary>
    /// To check if player can cross the road 
    /// </summary>
    public bool canCross;
    /// <summary>
    /// When vehicle traffic light is changing
    /// </summary>
    private bool lightChanging;
    [Header("Green man")]
    /// <summary>
    /// Time interval for blinking of green man
    /// </summary>
    public float greenManBlinkSpeed;
    /// <summary>
    /// Time for green man to start blinking
    /// </summary>
    public int timeToStartBlinking;

    /// <summary>
    /// Update all vehicle traffic lights and pedestrian crossing lights current state
    /// </summary>
    private void UpdateAllLights()
    {
        foreach (var pedestrianLight in pedestrianTrafficLights)
        {
            pedestrianLight.ChangeCrossingLight(pedestrianLightsCurrentState);
        }

        foreach (var light in trafficLights)
        {
            light.ChangeLight(trafficLightsCurrentState);
        }
    }

    /// <summary>
    /// Controls state transitions of a traffic light using a FSM
    /// Updates vehicle traffic light and pedestrian crossing light colour
    /// </summary>
    /// <param name="nextState">Vehicle traffic light next light colour</param>
    /// <returns></returns>
    IEnumerator ChangeLightStates(string nextState)
    {
        lightChanging = true;
        while (lightChanging)
        {
            switch (nextState)
            {
                case "greenLight":
                    trafficLightsCurrentState = "green";
                    pedestrianLightsCurrentState = "red";
                    UpdateAllLights();
                    yield return new WaitForSeconds(lightChangeTime);
                    nextState = "amberLight";
                    break;
                case "amberLight":
                    trafficLightsCurrentState = "amber";
                    pedestrianLightsCurrentState = "red";
                    UpdateAllLights();
                    yield return new WaitForSeconds(lightChangeTime);
                    nextState = "redLight";
                    break;
                case "redLight":
                    trafficLightsCurrentState = "red";
                    pedestrianLightsCurrentState = "green";
                    UpdateAllLights();
                    canCross = true;
                    yield return StartCoroutine(CrossingTimer());
                    nextState = "greenLight";
                    break;
            }
        }
    }

    /// <summary>
    /// Controls the time player needs to wait until traffic light starts to change colour after pressing the button
    /// </summary>
    /// <returns>Wait for that amount of seconds until light starts to change</returns>
    IEnumerator BeforeLightChange() 
    {
        yield return new WaitForSeconds(beforeLightChangeTime);

        StartCoroutine(ChangeLightStates("greenLight"));
    }

    /// <summary>
    /// Controls all pedestrian crossing lights' green man to start blinking 
    /// </summary>
    /// <returns>Blinking interval</returns>
    IEnumerator GreenManBlinking()
    {
        foreach (var pedestrianLight in pedestrianTrafficLights)
        {
            pedestrianLight.greenMan.SetActive(!pedestrianLight.greenMan.activeSelf);
            yield return new WaitForSeconds(greenManBlinkSpeed);
        }
    }

    /// <summary>
    /// Controls the time for when player can cross the road
    /// </summary>
    /// <returns>Timer countdown</returns>
    IEnumerator CrossingTimer()  
    {
        int timer = crossingTime;
        //start beeping audio 
        foreach (var pedestrianLight in pedestrianTrafficLights)
        {
            pedestrianLight.clipIndex++;
            pedestrianLight.beepingAudioSource.clip = pedestrianLight.beepingAudioClips[pedestrianLight.clipIndex];
            pedestrianLight.beepingAudioSource.Play();
        }
        while (timer > 0)
        {
            UpdateTimerUI(timer);
            yield return new WaitForSeconds(1);
            timer--;
            ///less than this amt of time then start blinking
            if (timer == timeToStartBlinking)
            {
                StartCoroutine(GreenManBlinking());
            }
        }
        //stop beeping audio
        foreach (var pedestrianLight in pedestrianTrafficLights)
        {
            pedestrianLight.beepingAudioSource.Stop();
            pedestrianLight.clipIndex = 0;
        }
        UpdateTimerUI(0);
        buttonPressed = false;
        lightChanging = false;
        canCross = false;

        trafficLightsCurrentState = "green";
        pedestrianLightsCurrentState = "red";
        UpdateAllLights();
    }

    /// <summary>
    /// Update all pedestrian crossing lights crossing timer
    /// </summary>
    /// <param name="timer"></param>
    private void UpdateTimerUI(int timer)
    {
        foreach(var pedestrianLight in pedestrianTrafficLights)
        {
            if(timer > 0)
            {
                pedestrianLight.crossingTimerText.text = timer.ToString();
            }
            else
            {
                pedestrianLight.crossingTimerText.text = null;
            }
        }
    }
    /// <summary>
    /// Controls when all the traffic light behaviours begin
    /// </summary>
    public void Crossing()
    {
        if (buttonPressed)
        {
            foreach (var pedestrianLight in pedestrianTrafficLights)
            {
                pedestrianLight.buttonLight.SetActive(true);
            }
            StartCoroutine(BeforeLightChange());
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

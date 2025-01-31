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
    public List<TrafficLight> trafficLights;
    public List<PedestrianTrafficLight> pedestrianTrafficLights;

    public string trafficLightsCurrentState = "green";
    public string pedestrianLightsCurrentState = "red";

    public bool buttonPressed;
    public int beforeLightChangeTime;
    public int lightChangeTime;

    public int crossingTime;
    public bool canCross;

    bool lightChanging;
    bool greenManBlinking;
    public float greenManBlinkSpeed;
    public int timeToStartBlinking;

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

    IEnumerator BeforeLightChange() //waiting time after pressing button
    {
        yield return new WaitForSeconds(beforeLightChangeTime);

        StartCoroutine(ChangeLightStates("greenLight"));
    }


    IEnumerator GreenManBlinking()
    {

        while(greenManBlinking)
        {
            foreach (var pedestrianLight in pedestrianTrafficLights)
            {
                pedestrianLight.greenMan.SetActive(!pedestrianLight.greenMan.activeSelf);
                yield return new WaitForSeconds(greenManBlinkSpeed);
            }
        }
    }


    IEnumerator CrossingTimer() //time for when player can cross the road, green man 
    {
        int timer = crossingTime;
        //start crossing beeping
        //stop button beep
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
                greenManBlinking = true;
                StartCoroutine(GreenManBlinking());
            }
        }
        //stop beeping
        foreach (var pedestrianLight in pedestrianTrafficLights)
        {
            pedestrianLight.beepingAudioSource.Stop();
            pedestrianLight.clipIndex = 0;
        }
        UpdateTimerUI(0);
        buttonPressed = false;
        lightChanging = false;
        greenManBlinking = false;
        canCross = false;

        trafficLightsCurrentState = "green";
        pedestrianLightsCurrentState = "red";
        UpdateAllLights();
    }

    void UpdateTimerUI(int timer)
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

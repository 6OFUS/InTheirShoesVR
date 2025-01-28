using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    public List<TrafficLight> trafficLights;
    public List<PedestrianTrafficLight> pedestrianTrafficLights;

    private string trafficLightsCurrentState = "green";
    private string pedestrianLightsCurrentState = "red";

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
                case "Green light":
                    trafficLightsCurrentState = "green";
                    pedestrianLightsCurrentState = "red";
                    UpdateAllLights();
                    yield return new WaitForSeconds(lightChangeTime);
                    nextState = "Amber light";
                    break;
                case "Amber light":
                    trafficLightsCurrentState = "amber";
                    pedestrianLightsCurrentState = "red";
                    UpdateAllLights();
                    yield return new WaitForSeconds(lightChangeTime);
                    nextState = "Red light";
                    break;
                case "Red light":
                    trafficLightsCurrentState = "red";
                    pedestrianLightsCurrentState = "green";
                    UpdateAllLights();
                    canCross = true;
                    yield return StartCoroutine(CrossingTimer());
                    nextState = "Green light";
                    break;
            }
        }
    }

    IEnumerator BeforeLightChange() //waiting time after pressing button
    {
        yield return new WaitForSeconds(beforeLightChangeTime);

        StartCoroutine(ChangeLightStates("Green light"));
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

        while(timer > 0)
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

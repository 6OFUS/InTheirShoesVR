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
    bool isCrossing;

    bool lightChanging;
    bool greenManBlinking;
    public float greenManBlinkSpeed;

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
                    yield return StartCoroutine(CrossingTimer());
                    nextState = "Green light";
                    break;
            }
        }
    }

    IEnumerator BeforeLightChange()
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

    IEnumerator CrossingTimer()
    {
        int timer = crossingTime;
        greenManBlinking = true;
        StartCoroutine(GreenManBlinking());
        while(timer > 0)
        {
            //put text here
            Debug.Log(timer);
            UpdateTimerUI(timer);
            yield return new WaitForSeconds(1);
            timer--;
        }
        buttonPressed = false;
        lightChanging = false;
        greenManBlinking = false;

        trafficLightsCurrentState = "green";
        pedestrianLightsCurrentState = "red";
        UpdateAllLights();
    }

    void UpdateTimerUI(int timer)
    {
        foreach(var pedestrianLight in pedestrianTrafficLights)
        {
            pedestrianLight.crossingTimerText.text = timer.ToString();
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

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
    public bool canCross;
    public int waitTimeForLightChange;

    public int crossingTime;

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

    public IEnumerator ChangeLightStates(string nextState)
    {
        switch (nextState)
        {
            case "Green light":
                trafficLightsCurrentState = "green";
                pedestrianLightsCurrentState = "red";
                UpdateAllLights();
                yield return new WaitForSeconds(waitTimeForLightChange);
                StartCoroutine(ChangeLightStates("Amber light"));
                break;
            case "Amber light":
                trafficLightsCurrentState = "amber";
                pedestrianLightsCurrentState = "red";
                UpdateAllLights();
                yield return new WaitForSeconds(waitTimeForLightChange);
                StartCoroutine(ChangeLightStates("Red light"));
                break;
            case "Red light":
                trafficLightsCurrentState = "red";
                pedestrianLightsCurrentState = "green";
                UpdateAllLights();
                yield return new WaitForSeconds(crossingTime);
                StartCoroutine(ChangeLightStates("Green light"));
                break;
        }
    }

    public void Crossing()
    {
        if (buttonPressed && !canCross)
        {
            //wait a few seconds

            StartCoroutine(ChangeLightStates("Green light"));

            //show timer text
            //when timer = 0
            //change to red man and traffic light change to green
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

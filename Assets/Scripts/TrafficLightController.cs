using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    public List<TrafficLight> trafficLights;

    public void ChangeAllLights(string color)
    {
        foreach (var light in trafficLights)
        {
            //change colour here, from green -> amber -> red
        }
    }

    IEnumerator ChangeLight()
    {

        yield return new WaitForSeconds(3);
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

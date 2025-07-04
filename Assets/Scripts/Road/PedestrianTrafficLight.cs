/*
    Author: Hui Hui
    Date: 27/1/2025
    Description: The PedestrianTrafficLight class is used to handle the pedestrian traffic lights for player to cross the road
*/
using Oculus.Haptics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PedestrianTrafficLight : MonoBehaviour
{
    /// <summary>
    /// Reference the TrafficLightController script
    /// </summary>
    private TrafficLightController trafficLightController;

    /// <summary>
    /// Red man light 
    /// </summary>
    [Header("Crossing lights")]
    public GameObject redMan;
    /// <summary>
    /// Green man light
    /// </summary>
    public GameObject greenMan;
    /// <summary>
    /// Pedestrian crossing button light
    /// </summary>
    public GameObject buttonLight;

    /// <summary>
    /// Crossing timer text UI
    /// </summary>
    [Header("UI")]
    public TextMeshProUGUI crossingTimerText;

    /// <summary>
    /// Audio source for beeping audios from pedestrian crossing button
    /// </summary>
    [Header("Audio")]
    public AudioSource beepingAudioSource;
    /// <summary>
    /// Array of beeping audios from pedestrian crossing button
    /// </summary>
    public AudioClip[] beepingAudioClips;
    /// <summary>
    /// Index for beepingAudioClips array
    /// </summary>
    public int clipIndex;
    /// <summary>
    /// Audio source for when pedestrian crossing button is pressed
    /// </summary>
    public AudioSource pressButton;

    /// <summary>
    /// Haptic clip for when player presses button
    /// </summary>
    [Header("Haptics")]
    public HapticClip pressButtonHaptic;
    /// <summary>
    /// Haptic clip player for button press
    /// </summary>
    private HapticClipPlayer hapticClipPlayer;

    /// <summary>
    /// When player presses pedestrian crossing button
    /// </summary>
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
            StartCoroutine(Beeping());
        }
    }

    /// <summary>
    /// Send haptic feedback to XR controller that interacts with the button
    /// </summary>
    /// <param name="args">Event interactions in XR Simple Interactable</param>
    public void ButtonHaptic(SelectEnterEventArgs args)
    {
        if(args.interactorObject is XRBaseInteractor)
        {
            XRBaseInteractor interactor = args.interactorObject as XRBaseInteractor;
            if(interactor.handedness == InteractorHandedness.Left)
            {
                hapticClipPlayer.Play(Controller.Left);
            }
            else if(interactor.handedness == InteractorHandedness.Right)
            {
                hapticClipPlayer.Play(Controller.Right);
            }
        }
        PressButton();
    }

    /// <summary>
    /// Create new haptic clip player for press button
    /// </summary>
    void Awake()
    {
        hapticClipPlayer = new HapticClipPlayer(pressButtonHaptic);
    }

    /// <summary>
    /// Controls when waiting beeping audio starts
    /// </summary>
    /// <returns></returns>
    IEnumerator Beeping()
    {
        beepingAudioSource.clip = beepingAudioClips[clipIndex];
        yield return null;
        beepingAudioSource.Play();
        trafficLightController.Crossing();
    }

    /// <summary>
    /// Controls state transitions of a pedestrian traffic light using a FSM
    /// Updates when red or green man is on
    /// </summary>
    /// <param name="currentState">Current crossing light colour</param>
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
}

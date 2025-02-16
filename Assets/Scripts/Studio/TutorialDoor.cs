/*
    Author: Alfred Kang
    Date: 31/1/2025
    Description: Tutorial door script
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoor : MonoBehaviour
{
    /// <summary>
    /// The hinge joint component controlling the door movement.
    /// </summary>
    public HingeJoint hinge;

    /// <summary>
    /// The UI element displayed when the tutorial is locked.
    /// </summary>
    public GameObject tutorialLockedUI;

    /// <summary>
    /// The UI element displayed when the tutorial is unlocked.
    /// </summary>
    public GameObject tutorialUnlockedUI;

    /// <summary>
    /// The audio source component responsible for playing door sounds.
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// The audio clip that plays when the door opens.
    /// </summary>
    public AudioClip doorOpen;

    /// <summary>
    /// The audio clip that plays when the door is locked.
    /// </summary>
    public AudioClip doorLock;

    /// <summary>
    /// Attempts to open the tutorial door based on the player's sign-in status.
    /// If the player is signed in, the door opens; otherwise, it remains locked.
    /// </summary>
    public void OpenDoor()
    {
        JointLimits limits = hinge.limits;
        if (GameManager.Instance.playerID != "")
        {
            audioSource.clip = doorOpen;
            audioSource.Play();
            limits.max = 90;
        }
        else
        {
            audioSource.clip = doorLock;
            audioSource.Play();
            limits.max = 0;
            Debug.Log("Sign in first");
        }
        hinge.limits = limits;
    }

    public void TutorialUnlocked()
    {
        tutorialLockedUI.SetActive(false);
        tutorialUnlockedUI.SetActive(true);
    }

    public void TutorialLocked()
    {
        tutorialLockedUI.SetActive(true);
        tutorialUnlockedUI.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance.playerID != "")
        {
            TutorialUnlocked();
        }
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

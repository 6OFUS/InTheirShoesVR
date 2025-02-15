using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoor : MonoBehaviour
{
    public HingeJoint hinge;
    public GameObject tutorialLockedUI;
    public GameObject tutorialUnlockedUI;
    private AudioSource audioSource;
    public AudioClip doorOpen;
    public AudioClip doorLock;

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

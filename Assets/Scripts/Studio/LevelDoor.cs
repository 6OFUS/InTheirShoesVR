/*
    Author: Alfred Kang
    Date: 31/1/2025
    Description: Door script that manages whether the player is eligible to enter the area
*/
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelDoor : MonoBehaviour
{
    /// <summary>
    /// The hinge joint component used to control the door's movement.
    /// </summary>
    public HingeJoint doorHinge;

    /// <summary>
    /// The name of the previous level, used to check if the player has completed it.
    /// </summary>
    public string prevLvlName;

    /// <summary>
    /// The keycard scanner associated with the door.
    /// </summary>
    public KeycardScanner scanner;

    /// <summary>
    /// The audio source component responsible for playing door sounds.
    /// </summary>
    public AudioSource doorAudioSource;

    /// <summary>
    /// The audio clip that plays when the door unlocks and opens.
    /// </summary>
    public AudioClip doorOpenClip;

    /// <summary>
    /// The audio clip that plays when the door is locked.
    /// </summary>
    public AudioClip doorLockClip;

    /// <summary>
    /// Attempts to open the door based on the player's progress.
    /// Checks if the previous level has been completed or if the keycard has been scanned.
    /// </summary>
    public void DoorOpen()
    {
        if(GameManager.Instance.playerLevelProgress.Count != 0)
        {
            //get previous level data to check if level completed
            var previousLevelData = GameManager.Instance.playerLevelProgress[prevLvlName];
            var currentLevelData = GameManager.Instance.playerLevelProgress[scanner.keycardTag];

            JointLimits limits = doorHinge.limits;
            if (previousLevelData.completed) //previous level completed
            {
                if (currentLevelData.doorUnlocked) //door unlocked is for current door, not previous one
                {
                    limits.max = 90;
                    doorAudioSource.clip = doorOpenClip;
                    doorAudioSource.Play();
                    Debug.Log("Door unlocked");
                }
                else //door locked
                {
                    limits.max = 0;
                    doorAudioSource.clip = doorLockClip;
                    doorAudioSource.Play();
                    Debug.Log("Door locked, scan card");
                }
            }
            else //previous level not completed
            {
                if (currentLevelData.doorUnlocked) // Check if keycard was scanned
                {
                    limits.max = 90;
                    doorAudioSource.clip = doorOpenClip;
                    doorAudioSource.Play();
                    Debug.Log("First door unlocked with keycard.");
                }
                else
                {
                    limits.max = 0;
                    doorAudioSource.clip = doorLockClip;
                    doorAudioSource.Play();
                    Debug.Log($"Door cannot be unlocked, complete the {prevLvlName} level or scan keycard.");
                }
            }
            doorHinge.limits = limits;  
            Debug.Log(doorHinge.limits.max);
        }
        else
        {
            Debug.Log("Sign in first");
        }
    }
}

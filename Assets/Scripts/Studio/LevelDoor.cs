using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelDoor : MonoBehaviour
{
    public HingeJoint doorHinge;
    public string prevLvlName;
    public KeycardScanner scanner;
    public AudioSource doorAudioSource;
    public AudioClip doorOpenClip;
    public AudioClip doorLockClip;
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

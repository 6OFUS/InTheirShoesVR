using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelDoor : MonoBehaviour
{
    public HingeJoint doorHinge;
    public string prevLvlName;
    public KeycardScanner scanner;
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
                    Debug.Log("Door unlocked");
                }
                else //door locked
                {
                    limits.max = 0;
                    Debug.Log("Door locked, scan card");
                }
            }
            else //previous level not completed
            {
                if (currentLevelData.doorUnlocked) // Check if keycard was scanned
                {
                    limits.max = 90;
                    Debug.Log("First door unlocked with keycard.");
                }
                else
                {
                    limits.max = 0;
                    Debug.Log($"Door cannot be unlocked, complete the {prevLvlName} level or scan keycard.");
                }
            }
            doorHinge.limits = limits;  
        }
        else
        {
            Debug.Log("Sign in first");
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

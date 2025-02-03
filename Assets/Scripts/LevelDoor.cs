using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelDoor : MonoBehaviour
{
    public HingeJoint doorHinge;
    public int doorLevelIndex;
    public bool doorLocked = true;

    public void DoorOpen()
    {
        if(GameManager.Instance.playerLevelProgress.Count != 0)
        {
            var previousLevelData = GameManager.Instance.playerLevelProgress.ElementAt(doorLevelIndex - 1);
            JointLimits limits = doorHinge.limits;
            if (previousLevelData.Value) //previous level completed
            {
                if (!doorLocked) //door unlocked
                {
                    limits.min = -90;
                    limits.max = 90;
                    Debug.Log("Door unlocked");
                }
                else //door locked
                {
                    limits.min = 0;
                    limits.max = 0;
                    Debug.Log("Door locked, scan card");
                }
            }
            else
            {
                Debug.Log($"Door cannot be unlocked, complete the {previousLevelData.Key} level");
                limits.min = 0;
                limits.max = 0;
            }
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

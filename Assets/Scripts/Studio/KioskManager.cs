/*
    Author: Alfred Kang
    Date: 31/1/2025
    Description: This script manages the different keycards that the player is suppose to obtain through progression
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KioskManager : MonoBehaviour
{
    /// <summary>
    /// Database
    /// </summary>
    Database database;

    /// <summary>
    /// dyslexia scene Locked
    /// </summary>
    public GameObject dyslexiaLocked;

    /// <summary>
    /// dyslexia scene Locked
    /// </summary>
    public GameObject dyslexiaUnlocked;

    /// <summary>
    /// LockedButtons
    /// </summary>
    public List<GameObject> lockedButtons;

    /// <summary>
    /// unlockedButtons
    /// </summary>
    public List<GameObject> unlockedButtons;

    /// <summary>
    /// locked icons
    /// </summary>
    public List<GameObject> doorLockedIcon;

    /// <summary>
    /// unlocked icons
    /// </summary>
    public List<GameObject> doorUnockedIcon;

    private Dictionary<string, (bool completed, bool doorUnlocked)> playerProgress;

    /// <summary>
    /// Unlocks the dyslexia button by enabling the unlocked version 
    /// and disabling the locked version.
    /// </summary>
    public void DyslexiaButtonUnlock()
    {
        dyslexiaLocked.SetActive(false);
        dyslexiaUnlocked.SetActive(true);
    }

    /// <summary>
    /// Resets all buttons to their locked state, ensuring that 
    /// locked buttons and door icons are visible while unlocked buttons are hidden.
    /// </summary>
    public void ResetButtons()
    {
        for(int i = 0; i < lockedButtons.Count; i++)
        {
            lockedButtons[i].SetActive(true);
            unlockedButtons[i].SetActive(false);
            doorLockedIcon[i].SetActive(true);
            doorUnockedIcon[i].SetActive(false);
        }
    }

    /// <summary>
    /// Unlocks buttons based on player progress.
    /// Levels that have been completed or their doors unlocked 
    /// will have their corresponding buttons activated.
    /// </summary>
    /// <returns>IEnumerator to allow for delayed execution.</returns>
    public IEnumerator UnlockButtons()
    {
        yield return new WaitForSeconds(3);
        playerProgress = GameManager.Instance.playerLevelProgress;
        bool nextLevelUnlocked = false;

        for (int i = 0; i < lockedButtons.Count; i++)
        {
            string levelName = database.levelNames[i];
            var levelData = playerProgress[levelName];

            if (levelData.completed || levelData.doorUnlocked) // Unlock buttons for completed levels
            {
                lockedButtons[i].SetActive(false);
                unlockedButtons[i].SetActive(true);
                doorLockedIcon[i].SetActive(false);
                doorUnockedIcon[i].SetActive(true);
            }
            else if (!nextLevelUnlocked) // Unlock the next level's button (first non-completed level)
            {
                lockedButtons[i].SetActive(false);
                unlockedButtons[i].SetActive(true);
                nextLevelUnlocked = true;
            }
            else // Keep all future levels locked
            {
                lockedButtons[i].SetActive(true);
                unlockedButtons[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Initializes the KioskManager by finding the Database component.
    /// If the player has a valid ID, it starts the UnlockButtons coroutine.
    /// </summary>
    void Start()
    {
        database = FindObjectOfType<Database>();
        if(GameManager.Instance.playerID != "")
        {
            Debug.Log("unlocking buttons");
            StartCoroutine(UnlockButtons());
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

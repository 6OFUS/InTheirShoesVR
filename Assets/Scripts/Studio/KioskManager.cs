using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KioskManager : MonoBehaviour
{
    Database database;

    public GameObject dyslexiaLocked;
    public GameObject dyslexiaUnlocked;

    public List<GameObject> lockedButtons;
    public List<GameObject> unlockedButtons;
    public List<GameObject> doorLockedIcon;
    public List<GameObject> doorUnockedIcon;

    private Dictionary<string, (bool completed, bool doorUnlocked)> playerProgress;

    public void DyslexiaButtonUnlock()
    {
        dyslexiaLocked.SetActive(false);
        dyslexiaUnlocked.SetActive(true);
    }


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

    public IEnumerator UnlockButtons()
    {
        yield return new WaitForSeconds(3);
        playerProgress = GameManager.Instance.playerLevelProgress;
        bool nextLevelUnlocked = false;

        for (int i = 0; i < lockedButtons.Count; i++)
        {
            string levelName = database.levelNames[i];
            var levelData = playerProgress[levelName];

            if (levelData.completed) // Unlock buttons for completed levels
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
    // Start is called before the first frame update
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

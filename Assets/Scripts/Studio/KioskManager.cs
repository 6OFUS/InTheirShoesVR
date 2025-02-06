using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KioskManager : MonoBehaviour
{
    public Database database;

    public GameObject dyslexiaLocked;
    public GameObject dyslexiaUnlocked;
    public List<GameObject> lockedButtons;
    public List<GameObject> unlockedButtons;

    private Dictionary<string, (bool completed, bool doorUnlocked)> playerProgress;

    public void DyslexiaButtonUnlock()
    {
        dyslexiaLocked.SetActive(false);
        dyslexiaUnlocked.SetActive(true);
    }

    public void ResetButtons()
    {
        foreach(GameObject lockedButton in lockedButtons)
        {
            lockedButton.SetActive(true);
        }
        foreach(GameObject unlockedButton in unlockedButtons)
        {
            unlockedButton.SetActive(false);
        }
    }

    public IEnumerator UnlockButtons()
    {
        yield return new WaitForSeconds(2);
        playerProgress = GameManager.Instance.playerLevelProgress;
        Debug.Log(playerProgress.Count);
        bool nextLevelUnlocked = false;

        for (int i = 0; i < lockedButtons.Count; i++)
        {
            string levelName = database.levelNames[i];

            var levelData = playerProgress[levelName];

            if (levelData.completed) // Unlock buttons for completed levels
            {
                lockedButtons[i].SetActive(false);
                unlockedButtons[i].SetActive(true);
                Debug.Log($"Button for {levelName} unlocked (completed level).");
            }
            else if (!nextLevelUnlocked) // Unlock the next level's button (first non-completed level)
            {
                lockedButtons[i].SetActive(false);
                unlockedButtons[i].SetActive(true);
                Debug.Log($"Button for {levelName} unlocked (next level to complete).");
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

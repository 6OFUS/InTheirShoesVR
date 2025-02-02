using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelProgress
{
    public bool isCompleted;
    public Dictionary<string, bool> levelsProgress;

    // Constructor to initialize all levels with completion status
    public PlayerLevelProgress(List<string> levelNames, bool initialStatus)
    {
        levelsProgress = new Dictionary<string, bool>();
        foreach (var level in levelNames)
        {
            levelsProgress.Add(level, initialStatus);
        }
    }
}

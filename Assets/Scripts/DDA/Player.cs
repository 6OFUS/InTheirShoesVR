/*
    Author: Malcom Goh
    Date: 16/2/2025
    Description: Player Script for data handling
*/
using System;
using System.Collections.Generic;

[Serializable]
public class Player
{
    /// <summary>
    /// The name of the player.
    /// </summary>
    public string Name;

    /// <summary>
    /// The email address of the player.
    /// </summary>
    public string Email;

    /// <summary>
    /// The date the player joined the game.
    /// </summary>
    public string DateJoined;

    /// <summary>
    /// The filename of the camera skin the player is using.
    /// </summary>
    public string CameraSkinFile;

    /// <summary>
    /// The filename of the phone skin the player is using.
    /// </summary>
    public string PhoneSkinFile;

    /// <summary>
    /// The points the player has accumulated.
    /// </summary>
    public int Points;

    /// <summary>
    /// The total playtime of the player in minutes.
    /// </summary>
    public int TotalPlayTime;

    /// <summary>
    /// The player's achievement data.
    /// </summary>
    public AchievementData Achievements = new AchievementData();

    /// <summary>
    /// The player's progress across different levels.
    /// </summary>
    public LevelProgress Progress = new LevelProgress();

    /// <summary>
    /// Initializes a new instance of the Player class.
    /// </summary>
    /// <param name="name">The name of the player.</param>
    /// <param name="email">The email of the player.</param>
    /// <param name="dateJoined">The date the player joined.</param>
    /// <param name="points">The points of the player.</param>
    /// <param name="totalPlayTime">The total playtime of the player.</param>
    /// <param name="cameraSkinFile">The camera skin file for the player.</param>
    /// <param name="phoneSkinFile">The phone skin file for the player.</param>
    public Player(string name, string email, string dateJoined, int points, int totalPlayTime, string cameraSkinFile, string phoneSkinFile)
    {
        this.Name = name;
        this.Email = email;
        this.DateJoined = dateJoined;

        this.CameraSkinFile = cameraSkinFile;
        this.PhoneSkinFile = phoneSkinFile;


        this.Points = points;
        this.TotalPlayTime = totalPlayTime;

        this.Achievements = new AchievementData();
        this.Progress = new LevelProgress();
    }
}

/// <summary>
/// Represents the achievement data of the player.
/// </summary>
[Serializable]
public class AchievementData
{
    /// <summary>
    /// The first achievement.
    /// </summary>
    public Achievement A1 = new Achievement("", false);

    /// <summary>
    /// The second achievement.
    /// </summary>
    public Achievement A2 = new Achievement("", false);

    /// <summary>
    /// The third achievement.
    /// </summary>
    public Achievement A3 = new Achievement("", false);

    /// <summary>
    /// The fourth achievement.
    /// </summary>
    public Achievement A4 = new Achievement("", false);

    /// <summary>
    /// The fifth achievement.
    /// </summary>
    public Achievement A5 = new Achievement("", false);

    /// <summary>
    /// The sixth achievement.
    /// </summary>
    public Achievement A6 = new Achievement("", false);
}

/// <summary>
/// Represents a single achievement of the player.
/// </summary>
[Serializable]
public class Achievement
{
    /// <summary>
    /// The date when the achievement was obtained.
    /// </summary>
    public string DateObtained = "";

    /// <summary>
    /// Indicates whether the achievement has been obtained.
    /// </summary>
    public bool Obtained = false;

    /// <summary>
    /// Initializes a new instance of the Achievement class.
    /// </summary>
    /// <param name="dateObtained">The date the achievement was obtained.</param>
    /// <param name="obtained">A flag indicating if the achievement has been obtained.</param>
    public Achievement(string dateObtained, bool obtained)
    {
        this.Obtained = obtained;
        this.DateObtained = dateObtained;
    }
}

/// <summary>
/// Represents the progress of the player in various levels.
/// </summary>
[Serializable]
public class LevelProgress
{
    /// <summary>
    /// The player's progress in the visual level.
    /// </summary>
    public LevelState Visual = new LevelState();

    /// <summary>
    /// The player's progress in the hearing level.
    /// </summary>
    public LevelState Hearing = new LevelState();

    /// <summary>
    /// The player's progress in the dyslexic level.
    /// </summary>
    public LevelState Dyslexic = new LevelState();

    /// <summary>
    /// The player's progress in the mobility level.
    /// </summary>
    public LevelState Mobility = new LevelState();

    /// <summary>
    /// Initializes a new instance of the LevelProgress class.
    /// </summary>
    public LevelProgress() { }
}

/// <summary>
/// Represents the state of the player's progress in a specific level.
/// </summary>
[Serializable]
public class LevelState
{
    /// <summary>
    /// Indicates whether the level is completed.
    /// </summary>
    public bool Completed = false;
    /// <summary>
    /// Indicates whether the door to the next level is unlocked.
    /// </summary>
    public bool DoorUnlocked = false;

    /// <summary>
    /// Initializes a new instance of the LevelState class.
    /// </summary>
    public LevelState() { }
}

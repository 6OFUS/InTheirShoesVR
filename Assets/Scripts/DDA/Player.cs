using System;
using System.Collections.Generic;

[Serializable]
public class Player
{
    public string Name;
    public string Email;
    public string DateJoined;
    public string PreferredTheme;
    public string ProfilePictureUrl;
    public string PhotoGalleryPath;
    public int Points;
    public int TotalPlayTime;

    public AchievementData Achievements = new AchievementData();

    public LevelProgress Progress = new LevelProgress();

    public Player(string name, string email, string dateJoined, string preferredTheme, string profilePictureUrl, string photoGalleryPath, int points, int totalPlayTime)
    {
        this.Name = name;
        this.Email = email;
        this.DateJoined = dateJoined;
        this.PreferredTheme = preferredTheme;
        this.ProfilePictureUrl = profilePictureUrl;
        this.PhotoGalleryPath = photoGalleryPath;
        this.Points = points;
        this.TotalPlayTime = totalPlayTime;

        this.Achievements = new AchievementData();
        this.Progress = new LevelProgress();
    }
}

[Serializable]
public class AchievementData
{
    public Achievement A1 = new Achievement("", false);
    public Achievement A2 = new Achievement("", false);
    public Achievement A3 = new Achievement("", false);
    public Achievement A4 = new Achievement("", false);
    public Achievement A5 = new Achievement("", false);
    public Achievement A6 = new Achievement("", false);
}


[Serializable]
public class Achievement
{
    public string DateObtained = "";
    public bool Obtained = false;

    public Achievement(string dateObtained, bool obtained)
    {
        this.Obtained = obtained;
        this.DateObtained = dateObtained;
    }
}

[Serializable]
public class LevelProgress
{
    public LevelState Visual = new LevelState();
    public LevelState Hearing = new LevelState();
    public LevelState Dyslexic = new LevelState();
    public LevelState Mobility = new LevelState();

    public LevelProgress() { }
}

[Serializable]
public class LevelState
{
    public bool Completed = false;
    public bool DoorUnlocked = false;

    public LevelState() { }
}

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

    public List<Achievement> Achievements = new List<Achievement>();
    public LevelProgress Progress = new LevelProgress();

    public Player(string name, string email, string dateJoined, string preferredTheme, string profilePictureUrl, string photoGalleryPath, int points)
    {
        this.Name = name;
        this.Email = email;
        this.DateJoined = dateJoined;
        this.PreferredTheme = preferredTheme;
        this.ProfilePictureUrl = profilePictureUrl;
        this.PhotoGalleryPath = photoGalleryPath;
        this.Points = points;

        Achievements = new List<Achievement>
        {
            new Achievement
            {
                AchievementID = "A1",
                Title = "Cloud Sync",
                Description = "Created an Account",
                PointsAwarded = 10,
                DifficultyLevel = "Easy",
                Category = "learning"
            }
        };

        Progress = new LevelProgress();
        Points = points;
    }
}

[Serializable]
public class Achievement
{
    public string AchievementID = "";
    public string Title = "";
    public string Description = "";
    public int PointsAwarded = 0;
    public string DifficultyLevel = ""; // easy, medium, hard
    public string Category = ""; // learning, sensory, physical

    public Achievement() { }
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

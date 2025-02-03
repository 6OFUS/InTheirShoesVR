using System;
using System.Collections.Generic;

public class Player
{
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime DateJoined { get; set; }
    public int TotalPlayTime { get; set; }
    public string PreferredTheme { get; set; }
    public string ProfilePictureUrl { get; set; }
    public string PhotoGalleryPath { get; set; }
    public List<Achievement> Achievements { get; set; }
    public LevelProgress Progress { get; set; }

    public Player(string name, string email, DateTime dateJoined, int totalPlayTime, string preferredTheme, string profilePictureUrl, string photoGalleryPath)
    {
        Name = name;
        Email = email;
        DateJoined = dateJoined;
        TotalPlayTime = totalPlayTime;
        PreferredTheme = preferredTheme;
        ProfilePictureUrl = profilePictureUrl;
        PhotoGalleryPath = photoGalleryPath;
        Achievements = new List<Achievement>();
        Progress = new LevelProgress();
    }
}

public class Achievement
{
    public string AchievementID { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int PointsAwarded { get; set; }
    public string DifficultyLevel { get; set; } // easy, medium, hard
    public string Category { get; set; } // learning, sensory, physical
}

public class LevelProgress
{
    public bool TutorialCompleted { get; set; }
    public bool VisualCompleted { get; set; }
    public bool HearingCompleted { get; set; }
    public bool DyslexiaCompleted { get; set; }
    public bool WheelchairCompleted { get; set; }
}
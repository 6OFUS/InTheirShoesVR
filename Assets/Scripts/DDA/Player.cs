using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string name;
    public string email;
    public string dateJoined;
    public int totalPlayTime;

    public Player(string name, string email, string dateJoined, int totalPlayTime)
    {
        this.name = name;
        this.email = email;
        this.dateJoined = dateJoined;
        this.totalPlayTime = totalPlayTime; 
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class User
{
    public string Name;
    public string Date;
    public string Victory;
    public float Time;
    public int Deaths;

    public User(string name, string date, string victory, float time, int deaths)
    {
        Name = name;
        Date = date;
        Victory = victory;
        Time = time;
        Deaths = deaths;
    }
}
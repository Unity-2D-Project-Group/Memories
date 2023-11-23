using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public static Level _instance;
    public int _levelID;

    public int _currentCheckpoint;
    public int _currentFragment;
    public bool _unlocked;
    public Level(int levelID, bool unlocked)
    {
        _instance = this;
        _levelID = levelID;
        _currentCheckpoint = 0;
        _currentFragment = 0;
        _unlocked = unlocked;
    }
}
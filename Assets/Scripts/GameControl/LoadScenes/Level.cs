using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public static Level _instance;
    public int _levelID;

    public int _currentCheckpoint;
    public int?[] _collectedFragments;
    public bool _unlocked;
    public Level(int levelID, bool unlocked)
    {
        _instance = this;
        _levelID = levelID;
        _currentCheckpoint = 0;
        //_currentFragment = -1;
        _collectedFragments = new int?[3];
        _unlocked = unlocked;
    }
}

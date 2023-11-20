using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Game
{
    public static Game _instance;
    public int _currentCheckpoint;
    public int _currentLevel;
    public int _currentFragment;
    public Game()
    {
        _instance = this;

        _currentCheckpoint = 0;
        _currentLevel = 1;
        _currentFragment = 0;
    }
}

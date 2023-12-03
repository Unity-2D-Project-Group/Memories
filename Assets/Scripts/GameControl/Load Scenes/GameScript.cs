using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Save
{
    public static Save _instance;
    public float _currentLevel;
    public Hashtable Levels = new Hashtable();
    public Save()
    {
        _instance = this;

        _currentLevel = 0;
        Levels["Level0"] = new Level(0, true);
        Levels["Level1"] = new Level(1, false);
        Levels["Level2"] = new Level(2, false);
        Levels["Level3"] = new Level(3, false);
    }


    public void FinishLevel(int level)
    {
        Level temp = (Level)_instance.Levels[$"Level{level}"];

        temp._currentFragment = -1;
        temp._currentCheckpoint = 0;
        _instance.Levels[$"Level{level}"] = temp;
        LoadingData.PlayingLevel = -1;

        if (_instance._currentLevel == temp._levelID)
        {
            PassLevel(temp);
        }
    }

    public void PassLevel(Level temp)
    {
        _instance._currentLevel++;

        temp = (Level)_instance.Levels[$"Level{_instance._currentLevel}"];
        temp._unlocked = true;
        _instance.Levels[$"Level{_instance._currentLevel}"] = temp;
    }
}

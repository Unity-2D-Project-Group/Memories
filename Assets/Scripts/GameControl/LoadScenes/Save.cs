using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

[System.Serializable]
public class Save
{
    public static Save _instance;
    public float _currentLevel;
    public LinkedList<Level> Levels = new LinkedList<Level>();
    public Save()
    {
        _instance = this;

        _currentLevel = 1;
        Levels.InsertAtBegin(new Level(3, false));
        Levels.InsertAtBegin(new Level(2, false));
        Levels.InsertAtBegin(new Level(1, true));
        Levels.InsertAtBegin(new Level(0, true));
    }


    public void FinishLevel(int level)
    {
        Level temp = (Level)_instance.Levels.getNode(level).data;

        for(int i = 0; i < temp._collectedFragments.Length; i++)
        {
            temp._collectedFragments[i] = null;
        }
        temp._currentCheckpoint = 0;
        _instance.Levels.getNode(level).data = temp;
        LoadingData.PlayingLevel = -1;

        if (_instance._currentLevel == temp._levelID)
        {
            PassLevel(temp);
        }
    }

    public void PassLevel(Level temp)
    {
        _instance._currentLevel++;

        temp = (Level)_instance.Levels.getNode((int)_instance._currentLevel).data;
        temp._unlocked = true;
        _instance.Levels.getNode((int)_instance._currentLevel).data = temp;
    }
}

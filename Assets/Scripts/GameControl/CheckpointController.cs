using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public static CheckpointController _instance;
    private GameObject _player;

    [Header("Components")]
    public Hashtable _checkpoints = new Hashtable();

    public void StartCheckPoints()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        Save._instance = SaveLoad._savedGame;
        Level temp = (Level)SaveLoad._savedGame.Levels[$"Level{SaveLoad._savedGame._currentLevel}"];

        foreach (CheckPoint checkPoint in FindObjectsOfType<CheckPoint>().ToList())
        {
            _checkpoints.Add(checkPoint._checkPointID.ToString(), checkPoint.gameObject);

            if (checkPoint._checkPointID <= temp._currentCheckpoint)
            {
                checkPoint._activated = false;
            }
        }

        TeleportToCheckPoint();
    }

    public void TeleportToCheckPoint()
    {
        Level tempLevel = (Level)SaveLoad._savedGame.Levels[$"Level{SaveLoad._savedGame._currentLevel}"];
        GameObject temp = (GameObject)_checkpoints[tempLevel._currentCheckpoint.ToString()];
        _player.transform.position = temp.transform.position;
    }
}

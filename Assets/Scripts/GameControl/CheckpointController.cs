using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public static CheckpointController Instance;
    private GameObject _player;

    [Header("Components")]
    public Hashtable _checkpoints = new Hashtable();

    public void StartCheckPoints()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        Game._instance = SaveLoad._savedGame;

        foreach(CheckPoint checkPoint in FindObjectsOfType<CheckPoint>().ToList())
        {
            _checkpoints.Add(checkPoint._checkPointID.ToString(), checkPoint.gameObject);

            if (checkPoint._checkPointID <= SaveLoad._savedGame._currentCheckpoint)
            {
                checkPoint._activated = false;
            }
        }

        TeleportToCheckPoint();
    }

    public void TeleportToCheckPoint()
    {
        GameObject temp = (GameObject)_checkpoints[SaveLoad._savedGame._currentCheckpoint.ToString()];
        _player.transform.position = temp.transform.position;
    }
}

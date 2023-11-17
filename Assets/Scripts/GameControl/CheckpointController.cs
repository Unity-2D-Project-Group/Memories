using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public static CheckpointController Instance;
    private GameObject _player;

    [Header("Components")]
    public List<CheckPoint> _checkpoints = new List<CheckPoint>();

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        Game._instance = SaveLoad._savedGame;

        _checkpoints = FindObjectsByType<CheckPoint>(FindObjectsSortMode.InstanceID).ToList();

        if(SaveLoad._savedGame._currentCheckpoint >= 1)
        {
            foreach(CheckPoint checkpoint in _checkpoints)
            {
                if(checkpoint._checkPointID <= SaveLoad._savedGame._currentCheckpoint)
                {
                    checkpoint._activated = false;
                }
            }
        }
    }

    public void TeleportToCheckPoint()
    {
        print(SaveLoad._savedGame._currentCheckpoint);
        _player.transform.position = _checkpoints[SaveLoad._savedGame._currentCheckpoint].transform.position;
    }
}

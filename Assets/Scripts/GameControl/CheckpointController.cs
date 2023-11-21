using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public static CheckpointController Instance;
    private GameObject _player;

    [Header("Components")]
    [SerializeField] private Hashtable _checkpoints = new Hashtable();
    //[SerializeField] private List<CheckPoint> _checkpoints = new List<CheckPoint>();

    public void StartCheckPoints()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        Game._instance = SaveLoad._savedGame;

        foreach(GameObject checkPoint in GameObject.FindGameObjectsWithTag("CheckPoint"))
        {
            _checkpoints.Add(checkPoint.GetComponent<CheckPoint>()._checkPointID.ToString(), checkPoint);
            //print(_checkpoints[checkPoint.GetComponent<CheckPoint>()._checkPointID.ToString()]);

            if (checkPoint.GetComponent<CheckPoint>()._checkPointID <= SaveLoad._savedGame._currentCheckpoint)
            {
                checkPoint.GetComponent<CheckPoint>()._activated = false;
            }
        }

        TeleportToCheckPoint();
    }

    public void TeleportToCheckPoint()
    {
        print(SaveLoad._savedGame._currentCheckpoint);
        GameObject temp = (GameObject)_checkpoints[SaveLoad._savedGame._currentCheckpoint.ToString()];
        _player.transform.position = temp.transform.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public static CheckpointController _instance;

    [Header("Components")] 
    private GameObject _player;
    public Hashtable _checkpoints = new Hashtable();

    private void Start()
    {
        //Get the components
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public void StartCheckPoints()
    {
        Save._instance = SaveLoad._savedGame;
        //Get the current level info
        Level temp = (Level)SaveLoad._savedGame.Levels[$"Level{LoadingData.PlayingLevel}"];

        //Search for all the Checkpoints on the scene
        foreach (CheckPoint checkPoint in FindObjectsOfType<CheckPoint>().ToList())
        {
            //Save the checkpoints in the list
            _checkpoints.Add(checkPoint._checkPointID.ToString(), checkPoint.gameObject);

            //If the Checkpoint was already collected before, set it to unactive
            if (checkPoint._checkPointID <= temp._currentCheckpoint)
            {
                checkPoint._activated = false;
                checkPoint.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }

        //Teleport the player to the checkpoint
        TeleportToCheckPoint(temp);
    }

    public void TeleportToCheckPoint(Level level)
    {
        GameObject temp = (GameObject)_checkpoints[level._currentCheckpoint.ToString()];
        _player.transform.position = temp.transform.position;
    }
}
